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
        private readonly ApplicationDbContext _db;

        public TrailRepository(ApplicationDbContext db)
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

        public Trail GetTrail(int trailId)
        {
            return _db.Trails?.Include(t => t.NationalPark)?.FirstOrDefault(n => n.Id == trailId);
        }

        public ICollection<Trail> GetTrails()
        {
            return _db.Trails?.Include(t => t.NationalPark)?.OrderBy(n => n.Name)?.ToList();
        }

        public bool TrailExists(string name)
        {
            bool value = _db.Trails?.Any(n => n.Name.ToLower().Trim() == name.ToLower().Trim()) ?? false;
            return value;
        }

        public bool TrailExists(int id)
        {
            return _db.Trails?.Any(n => n.Id == id) ?? false;
        }

        public bool Save()
        {
            return _db.SaveChanges() >= 0;
        }

        public bool UpdateTrail(Trail trail)
        {
            _db.Trails.Update(trail);
            return Save();
        }

        public ICollection<Trail> GetTrailsInNationalPark(int nationalParkId)
        {
            return _db.Trails?.Include(t => t.NationalPark)?.Where(t => t.NationalParkId == nationalParkId)?.ToList();
        }
    }
}
