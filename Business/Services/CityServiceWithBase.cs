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
    public interface ICityService : IService<CityModel, City, ZamazorContext>
    {
        Result<List<CityModel>> List(int countryId);
    }

    public class CityService : ICityService
    {
        public RepoBase<City, ZamazorContext> Repo { get; set; } = new Repo<City, ZamazorContext>();

        public Result Add(CityModel model)
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

        public IQueryable<CityModel> Query()
        {
            return Repo.Query("Country").OrderBy(c => c.Name).Select(c => new CityModel()
            {
                Name = c.Name,
                Id = c.Id,
                CountryId = c.CountryId,

                CountryNameDisplay = c.Country.Name
            });
        }

        public Result Update(CityModel model)
        {
            if (Query().Any(c => c.Name.ToUpper() == model.Name.ToUpper().Trim() && c.Id != model.Id))
                return new ErrorResult("Girdiğiniz şehir adına sahip kayıt bulunmaktadır!");
            City entity = Repo.Query(c => c.Id == model.Id).SingleOrDefault();
            entity.Name = model.Name.Trim();
            entity.CountryId = model.CountryId;
            Repo.Update(entity);
            return new SuccessResult();
        }

        // select * from Cities where countryId = 1
        public Result<List<CityModel>> List(int countryId)
        {
            var list = Query().Where(c => c.CountryId == countryId).ToList();
            return new SuccessResult<List<CityModel>>(list);
        }
    }
}
