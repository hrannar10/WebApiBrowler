using System;
using System.Collections.Generic;
using WebApiBrowler.Entities;
using WebApiBrowler.Helpers;

namespace WebApiBrowler.Services
{
    public interface ICompanyService
    {
        IEnumerable<Company> GetAll();
        Company GetById(int id);
        List<AppUser> GetUsersById(int id);
        void AddUser(int id, Guid identityId);
        void RemoveUser(int id, Guid identityId);
        Company Create(Company company);
        bool Update(Company companyParm);
        bool Delete(int id);
    }

    public class CompanyService : ICompanyService
    {
        private readonly ApplicationDbContext _context;

        public CompanyService(
            ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Company> GetAll()
        {
            return _context.Companies;
        }

        public Company GetById(int id)
        {
            return _context.Companies.Find(id);
        }

        public List<AppUser> GetUsersById(int id)
        {
            throw new System.NotImplementedException();
        }

        public void AddUser(int id, Guid identityId)
        {
            var company = _context.Companies.Find(id);
            company.UserIds.Add(identityId);

            _context.Companies.Update(company);
            _context.SaveChanges();
        }

        public void RemoveUser(int id, Guid identityId)
        {
            var company = _context.Companies.Find(id);
            
            // validation
            if (company == null)
            {
                throw new AppException("Company not found");
            }

            var user = _context.Users.Find(identityId);

            if (user == null)
            {
                throw new AppException("User not found");
            }

            company.UserIds.Remove(identityId);

            _context.Companies.Update(company);
            _context.SaveChanges();
        }

        public Company Create(Company company)
        {
            // validation
            if (company == null)
            {
                throw new AppException("Company not found");
            }
            _context.Companies.Add(company);
            _context.SaveChanges();

            return company;
        }

        public bool Update(Company companyParm)
        {
            var company = _context.Companies.Find(companyParm.Id);

            if (company == null)
            {
                throw new AppException("Company not found");
            }

            // update company properties
            company.Name = companyParm.Name;

            _context.Companies.Update(company);
            _context.SaveChanges();
            return true;
        }

        public bool Delete(int id)
        {
            var company = _context.Companies.Find(id);
            if (company == null) return false;

            _context.Companies.Remove(company);
            _context.SaveChanges();

            return true;
        }
    }
}
