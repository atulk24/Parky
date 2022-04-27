using Microsoft.EntityFrameworkCore;
using ParkyAPI.Data;
using ParkyAPI.Models;
using ParkyAPI.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyAPI.Repository
{
    public class TrailRepository : ITrailRepository
    {
        private readonly ApplicationDBContext _db;

        public TrailRepository(ApplicationDBContext db)
        {
            _db = db;
        }
        public bool CreateTrail(Trail trail)
        {
            _db.Trails.Add(trail);
            return Save();
        }

        public bool DeleteTrail(Trail trail)
        {
            _db.Trails.Remove(trail);     
            return Save();
        }

        public bool UpdateTrail(Trail trail)
        {
            _db.Trails.Update(trail);
            return Save();
        }

        public ICollection<Trail> GetTrail()
        {
            return _db.Trails.Include(c => c.NationalPark).OrderBy(x => x.Name).ToList();
        }

        public Trail GetTrail(int TrailId)
        {
            return _db.Trails.Include(c => c.NationalPark).FirstOrDefault(x => x.Id == TrailId);
        }

        public bool TrailExists(string name)
        {
            bool value = _db.Trails.Any(x => x.Name.ToLower().Trim() == name.ToLower().Trim());
            return value;
        }

        public bool TrailExists(int Id)
        {
            bool value = _db.Trails.Any(x => x.Id == Id);
            return value;
        }

        public bool Save()
        {
            return _db.SaveChanges() >= 0 ? true: false;
        }

        public ICollection<Trail> GetTrailsInNationalPark(int npId)
        {
            return _db.Trails.Include(c => c.NationalPark).Where(c => c.NationalParkId == npId).ToList();
        }
    }
}
