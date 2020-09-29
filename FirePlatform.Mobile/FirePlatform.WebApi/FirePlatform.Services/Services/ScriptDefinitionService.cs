using FirePlatform.Models.Models;
using FirePlatform.Repositories;
using FirePlatform.Repositories.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirePlatform.Services.Services
{
    public class ScriptDefinitionService : BaseService<ScriptDefinitionService, ScriptDefinitionRepository, ScriptDefinition>
    {
        public ScriptDefinitionService
           (
               BaseRepository<ScriptDefinition, ScriptDefinitionRepository> baseRepository,
               Repository repository
           ) : base(baseRepository, repository)
        {
        }

        public async Task<bool> SaveAll(IEnumerable<ScriptDefinition> items)
        {
            try
            {
                var context = Repository.GetScriptDefinitionRepository();
                await context.CreateRange(items);
                return true;
            }
            catch (Exception ex)
            {
               
            }
            return false;
        }

        public Task<IEnumerable<ScriptDefinition>> GetAsync()
        {
            try
            {
                var context = Repository.GetScriptDefinitionRepository();
                return context.GetAsync();
            }
            catch (Exception ex)
            {
               
            }
            return null;
        }

        public async Task<bool> RemoveAll()
        {
            try
            {
                var context = Repository.GetScriptDefinitionRepository();
                await context.DeleteAllAsync();
                return true;
            }
            catch (Exception ex)
            {
                
            }
            return false;
        }

        public IEnumerable<ScriptDefinition> LoadRemoteScripts()
        {
            var result = new List<ScriptDefinition>();
            string data = string.Empty;
            using (var wc = new System.Net.WebClient())
            {
                data = wc.DownloadString("https://docs.google.com/spreadsheets/d/1hh0mYlkmSvRQwgiIhAnnEOmHKCpg293empAKl1Kj2mc/export?format=csv&id=1hh0mYlkmSvRQwgiIhAnnEOmHKCpg293empAKl1Kj2mc&gid=0");
            }
            var items = data?.Split("\r\n").ToArray();
            if (items != null)
                for (int i = 1; i < items.Length; i++)
                {
                    var item = items[i];
                    var parts = ParseExcelLine(item);
                    result.Add(new ScriptDefinition()
                    {
                        Lng = parts[0],
                        ShortName = parts[1],
                        LongName = parts[2],
                        Country = parts[3],
                        Type = parts[4],
                        Title = parts[5],
                        Link = parts[6]
                    });
                }
            return result;
        }

        private string[] ParseExcelLine(string line)
        {
            if (string.IsNullOrWhiteSpace(line)) return null;
            List<string> result = new List<string>();

            string buffor = "";
            bool isString = false;
            foreach (var chr in line)
            {
                if (chr == '"')
                {
                    isString = !isString;
                    continue;
                }

                if (!isString && chr == ',')
                {
                    result.Add(buffor);
                    buffor = "";
                    continue;
                }
                buffor += chr;
            }
            result.Add(buffor);
            return result?.ToArray();
        }
    }
}
