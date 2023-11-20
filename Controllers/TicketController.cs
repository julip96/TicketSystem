using TicketSystem.Data;
using TicketSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace TicketSystem.Controllers
{
    [Authorize(Roles ="user, admin, dev")]
    public class TicketController : Controller
    {
        private readonly ApplicationDbContext _db;

        public TicketController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            if(User.IsInRole("user"))
            {
                IEnumerable<Ticket> objTicketList = _db.Tickets;
                return View(objTicketList);
            }
            // change to return home
            else
            {
				IEnumerable<Ticket> objTicketList = _db.Tickets;
				return View(objTicketList);
			}
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(string searchString)
        {
            // I make the same as above, but I need to use searchString to filter the db of the user OR all in case of admin
            //Model.Where(x => x.OwnerID == User.FindFirstValue(ClaimTypes.NameIdentifier))


            IEnumerable<Ticket> objTicketList = _db.Tickets;
			// Convert searchString to an integer (assuming it's a valid integer)
			int searchInt;
			bool isNumeric = int.TryParse(searchString, out searchInt);
			objTicketList = objTicketList
                .Where(x =>
                {
                    // in case of dev providing blank editet content
                    if(x == null)
                    {
                        return false;
                    }
					if (x.TicketContent == null)
					{
						return false;
					}
					if(x.TicketSubject == null)
					{
						return false;
					}

					return x.TicketContent.Contains(searchString) ||
			        x.TicketSubject.Contains(searchString) ||
			        x.Status.ToString().Contains(searchString, StringComparison.OrdinalIgnoreCase) ||
			        (x.ID.ToString().Contains(searchString));

		        });
                
            if(!objTicketList.Any())
            {
                // I want a view here that says that nothing was found
                TempData["NoResultsMessage"] = "No results found for the given search.";
            }

			return View(objTicketList);
        }
        public IActionResult Solved()
        {

            return RedirectToAction("Index");
        }
        //GET
        public IActionResult Create()
        {
            return View();
        }
        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Ticket obj)
        {
            if(ModelState.IsValid) 
            {
            obj.OwnerID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            obj.CreationTime = DateTime.Now;
            obj.Status = (ProblemStatus)1;
            _db.Tickets.Add(obj);
            _db.SaveChanges();
            TempData["success"] = "Ticket created successfully";
            return RedirectToAction("Index");
            }
            return View(obj);
        }

        //GET
        public IActionResult Edit(int? id)
        {
            if(id==null || id==0)
            {
                return NotFound();
            }
            var ticketFromDb = _db.Tickets.Find(id);

            if(ticketFromDb == null)
            {
                return NotFound();
            }
            return View(ticketFromDb);
        }
        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Ticket updatedTicket)
        {
            if (ModelState.IsValid)
            {

                var ticketfromDb = _db.Tickets.Find(updatedTicket.ID);

                if(ticketfromDb == null)
                {
                    return NotFound();
                }

                ticketfromDb.TicketSubject = updatedTicket.TicketSubject;
                ticketfromDb.TicketContent = updatedTicket.TicketContent;
                ticketfromDb.Status = updatedTicket.Status;

                _db.SaveChanges();
    
                TempData["success"] = "Ticket edited successfully";
                return RedirectToAction("Index");
            }
            return View(updatedTicket);
        }
        //GET
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var ticketFromDb = _db.Tickets.Find(id);

            if (ticketFromDb == null)
            {
                return NotFound();
            }
            return View(ticketFromDb);
        }
        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Ticket obj)
        {
                _db.Tickets.Remove(obj);
                _db.SaveChanges();
                TempData["success"] = "Ticket deleted successfully";
                return RedirectToAction("Index");
        }
    }
}
