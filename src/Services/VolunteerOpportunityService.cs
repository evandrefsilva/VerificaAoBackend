//using System;
//using System.Collections.Generic;
//using System.Linq;
//using Data.Candidates;
//using Data.Context;
//using Microsoft.EntityFrameworkCore;
//using Services.Models;
//using X.PagedList;

//namespace Services
//{
//    public interface IVolunteerOpportunityService
//    {
//        AppResult GetAllOpportunities(int page = 1, int take = 30);
//        VolunteerOpportunityDTO GetOpportunityById(Guid id);
//        void AddOpportunity(VolunteerOpportunityForCreationDTO model, string cover);
//        void UpdateOpportunity(VolunteerOpportunityForUpdateDTO model);
//        void DeleteOpportunity(int id);
//    }

//    public class VolunteerOpportunityService : IVolunteerOpportunityService
//    {
//        private readonly DataContext _context;

//        public VolunteerOpportunityService(DataContext context)
//        {
//            _context = context;
//        }

//        public AppResult GetAllOpportunities(int page, int take)
//        {
//            var res = new AppResult();
//            var opportunities = _context.VolunteerOpportunities
//                .Select(o => new VolunteerOpportunityDTO
//                {
//                    Id = o.Id,
//                    Title = o.Title,
//                    Description = o.Description,
//                    StartDate = o.StartDate,
//                    EndDate = o.EndDate
//                })
//                .ToPagedList(page, take);

//            return res.Good(opportunities);
//        }

//        public VolunteerOpportunityDTO GetOpportunityById(Guid id)
//        {
//            var opportunity = _context.VolunteerOpportunities
//                .Where(o => o.Id == id)
//                .Select(o => new VolunteerOpportunityDTO
//                {
//                    Id = o.Id,
//                    Title = o.Title,
//                    Description = o.Description,
//                    StartDate = o.StartDate,
//                    EndDate = o.EndDate
//                })
//                .FirstOrDefault();

//            return opportunity;
//        }

//        public void AddOpportunity(VolunteerOpportunityForCreationDTO model, string cover)
//        {
//            var opportunity = new VolunteerOpportunity
//            {
//                Title = model.Title,
//                Description = model.Description,
//                StartDate = model.StartDate,
//                EndDate = model.EndDate,
//                CoverUrl = cover ?? cover.Replace(@"\\", "/")
//            };

//            _context.VolunteerOpportunities.Add(opportunity);
//            _context.SaveChanges();
//        }

//        public void UpdateOpportunity(VolunteerOpportunityForUpdateDTO model)
//        {
//            var opportunity = _context.VolunteerOpportunities.Find(model.Id);
//            if (opportunity != null)
//            {
//                opportunity.Title = model.Title;
//                opportunity.Description = model.Description;
//                opportunity.StartDate = model.StartDate;
//                opportunity.EndDate = model.EndDate;

//                _context.SaveChanges();
//            }
//        }

//        public void DeleteOpportunity(int id)
//        {
//            var opportunity = _context.VolunteerOpportunities.Find(id);
//            if (opportunity != null)
//            {
//                _context.VolunteerOpportunities.Remove(opportunity);
//                _context.SaveChanges();
//            }
//        }
//    }
//}
