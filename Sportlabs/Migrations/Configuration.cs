namespace SportsLabs.Migrations
{
    using SportsLabs.Models;
    using SportsLabs.DAL;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.IO;
    using System.Web.Hosting;
    using System.Text;
    using System.Data.Entity.Validation;
    using SportsLabs.Logging;


    internal sealed class Configuration : DbMigrationsConfiguration<TeamContext>
    {
        private ILogger _logger = new Logger();
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(TeamContext context)
        {
            try
            {
                var fileContents = GetFileStreamAsync();
                var lines = ReadAllLines(() => fileContents).ToList();
                var teams = lines
                    .Skip(1)
                    .Select(x => x.Split(new string[] { @",", "\r\n", "\n", "]", "\"", "}" }, StringSplitOptions.RemoveEmptyEntries))
                    .Select(x => new Team
                    {
                        Id = int.Parse(x[0].Trim().Replace("NULL", "0")),
                        Name = x[1].Trim().Replace("NULL", "Unknown"),
                        Country = x[2].Trim().Replace("NULL", "Unknown"),
                        Eliminated = x[3].Trim().ToLower().Replace("null", "false") == "true" ? true : false,
                    })
                .Cast<Team>().ToList();

                teams.ForEach(t => context.Teams.AddOrUpdate(x => x.Id, t));
                context.SaveChanges();

            }
            catch (DbEntityValidationException ex)
            {
                foreach (var eve in ex.EntityValidationErrors)
                {
                    _logger.Error(eve.ToString(), "Error executing command: data seed");

                    _logger.Error("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                     eve.Entry.Entity.GetType().Name, eve.Entry.State);

                    foreach (var ve in eve.ValidationErrors)
                    {
                        _logger.Error("- Property: \"{0}\", Error: \"{1}\"",
                        ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
        }





        public Stream GetFileStreamAsync()
        {
            var physicalPath = GetPhysicalPath();

            if (!File.Exists(physicalPath))
                throw new Exception($"Cannot get file stream because the file '{physicalPath}' does not exist.");

            var stream = File.OpenRead(physicalPath);


            return stream;
        }


        public string[] GetFileLines()
        {
            var physicalPath = GetPhysicalPath();

            if (!File.Exists(physicalPath))
                throw new Exception($"Cannot get file stream because the file '{physicalPath}' does not exist.");

            var lines = File.ReadAllLines(physicalPath);
            return lines;
        }

        private string GetPhysicalPath()
        {
            return HostingEnvironment.MapPath(@"~/App_Data/teams.csv");
        }


        public IEnumerable<string> ReadAllLines(Func<Stream> streamProvider)
        {
            var encoding = Encoding.UTF8;
            using (var stream = streamProvider())
            {
                using (var reader = new StreamReader(stream, encoding))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        yield return line;
                    }
                }
            }
        }
    }
}
