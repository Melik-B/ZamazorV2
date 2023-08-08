using AppCore.Business.Models.Results;
using AppCore.Business.Services.Bases;
using AppCore.DataAccess.EntityFramework.Bases;
using AppCore.DataAccess.EntityFramework;
using Business.Models;
using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Contexts;

namespace Business.Services
{
    public interface ICountryService : IService<CountryModel, Country, ZamazorContext>
    {

    }

    public class CountryService : ICountryService
    {
        public RepoBase<Country, ZamazorContext> Repo { get; set; } = new Repo<Country, ZamazorContext>();

        public Result Add(CountryModel model)
        {
            throw new NotImplementedException();
        }

        public Result Delete(int id)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            Repo.Dispose();
        }

        public IQueryable<CountryModel> Query()
        {
            return Repo.Query().OrderBy(c => c.Name).Select(c => new CountryModel()
            {
                Name = c.Name,
                Id = c.Id
            });
        }

        public Result Update(CountryModel model)
        {
            throw new NotImplementedException();
        }
    }
}
