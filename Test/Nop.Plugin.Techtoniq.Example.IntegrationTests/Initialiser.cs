using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json;
using Nop.Data;
using NUnit.Framework;

namespace Nop.Plugin.Techtoniq.Example.IntegrationTests
{
    [SetUpFixture]
    public class Initialiser : IntegrationTestBase
    {
        private static readonly string DatabaseServerName = "localhost\\SQLEXPRESS";
        private static readonly string DatabaseNamePrefix = "nopCommerceIntegrationDb-";

        [OneTimeSetUp]
        public async Task Init()
        {
            ResetDataSettings();

            System.IO.Directory.CreateDirectory(@$"{TestFolderRoot}\App_Data\Localization");
            SymbolicLink.CreateJunction(@$"{TestFolderRoot}\App_Data\Localization\Installation", @$"{NopWebRoot}\..\App_Data\Localization\Installation");

            var formValues = new Dictionary<string, string>();
            formValues.Add("AdminEmail", "admin@yourStore.com");
            formValues.Add("AdminPassword", "AdminPassword");
            formValues.Add("ConfirmPassword", "AdminPassword");
            formValues.Add("InstallSampleData", "true");
            formValues.Add("DataProvider", DataProviderType.SqlServer.ToString());
            formValues.Add("CreateDatabaseIfNotExists", "true");
            formValues.Add("ServerName", DatabaseServerName);
            formValues.Add("DatabaseName", $"{DatabaseNamePrefix}{DateTime.UtcNow.ToString("yyyy-MM-dd-HHmmss")}");
            formValues.Add("IntegratedSecurity", "true");
            FormUrlEncodedContent form = new FormUrlEncodedContent(formValues);

            // We use our own instance of the web application for initialisation of the database, because
            // nopCommerce requires a restart after database installation.
            // So the integration tests can all share the main singleton web app instance.
            var factory = new CustomWebApplicationFactory<Nop.Web.Startup>(TestFolderRoot, NopWebRoot);
            var client = factory.Server.CreateClient();
            var response = await client.PostAsync("/install", form);
            response.StatusCode.Should().Be(200);

            // Database now installed, clear the settings cache so that the next time nopCommerce checks to see 
            // if the database is installed, it does a proper check instead of reading the value from the cache.
            DataSettingsManager.ResetCache();
        }

        private void ResetDataSettings()
        {
            System.IO.Directory.CreateDirectory(@$"{TestFolderRoot}\App_Data");
            var settings = new DataSettings();
            settings.ConnectionString = string.Empty;
            var json = JsonConvert.SerializeObject(settings, Formatting.Indented);
            File.WriteAllText($@"{TestFolderRoot}\App_Data\dataSettings.json", json);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            var databaseNames = GetTestDatabaseNames();
            DeleteDatabases(databaseNames);
        }

        private List<string> GetTestDatabaseNames()
        {
            var server = new Microsoft.SqlServer.Management.Smo.Server(DatabaseServerName);

            var databaseNames = new List<string>();
            foreach (var db in server.Databases)
            {
                var dbName = db.ToString().Replace("[", "").Replace("]", "");
                if (dbName.StartsWith(DatabaseNamePrefix))
                {
                    databaseNames.Add(dbName);
                }
            }
            return databaseNames;
        }

        private void DeleteDatabases(List<string> databaseNames)
        {
            var server = new Microsoft.SqlServer.Management.Smo.Server(DatabaseServerName);
            foreach (var dbName in databaseNames)
            {
                server.KillAllProcesses(dbName);
                server.KillDatabase(dbName);
            }
        }
    }
}