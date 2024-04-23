using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC_Grupp_5.Data;
using MVC_Grupp_5.Models;

namespace MVC_Grupp_5.Controllers
{
    public class VehiclesController : Controller
    {
        private readonly MVC_Grupp_5Context _context;

        public VehiclesController(MVC_Grupp_5Context context)
        {
            _context = context;
        }

        // GET: Vehicles
        //public async Task<IActionResult> Index()
        //{
        //    return View(await _context.Vehicle.ToListAsync());
        //}
        public async Task<IActionResult> Index()
        {
            var vehicles = await _context.Vehicle.ToListAsync();
            return View(vehicles);
        }

        //public async Task<IActionResult> Index()
        //{
        //    var model = _context.Vehicle.Select(v => new Vehicle
        //    {
        //        RegNr = v.RegNr,
        //        Model = v.Model,
        //        Color = v.Color,
        //        VehicleType = v.VehicleType,
        //        CheckInVehicle = v.CheckInVehicle,

        //    });
        //    return View("Index", await model.ToListAsync());
        //}

        // GET: Vehicles/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicle = await _context.Vehicle
                .FirstOrDefaultAsync(m => m.RegNr == id);
            if (vehicle == null)
            {
                return NotFound();
            }

            return View(vehicle);
        }

        // GET: Vehicles/Create
        public IActionResult Create()
        {
            return View(new Vehicle());
        }

        // POST: Vehicles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RegNr,Model,Color,VehicleType")] Vehicle vehicle)
        {
            if (ModelState.IsValid)
            {
                var existingVehicle = await _context.Vehicle.FirstOrDefaultAsync(v => v.RegNr == vehicle.RegNr);
                if (existingVehicle != null)
                {
                    ModelState.AddModelError("RegNr", "This Registration number is already checked in");
                    return View("Create", vehicle);
                }
                vehicle.CheckInVehicle = DateTime.Now;
                if(string.IsNullOrWhiteSpace(vehicle.RegNr))
                {
                    ModelState.AddModelError("RegNr", "Registration number is required");
                    return View("Create", vehicle);
                }
                if (vehicle.RegNr.Contains("-"))
                {
                    ModelState.AddModelError("RegNr", "Registration number can not contain negative values");
                    return View("Create", vehicle);
                }
                _context.Add(vehicle);
                await _context.SaveChangesAsync();

                //Success message will be dispalyed in CheckInSuccess view
                ViewBag.SuccessMessage = $"{vehicle.RegNr}: Checked In Successflully";

                return RedirectToAction("CheckInSuccess", new {regNr = vehicle.RegNr });
            }
            return View("Create", vehicle);
        }

        public IActionResult CheckInSuccess(string regNr)
        {
            ViewBag.RegNr = regNr;
            return View("~/Views/FeedbackToUser/CheckInSuccess.cshtml");
        }


        // GET: Vehicles/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicle = await _context.Vehicle.FindAsync(id);
            if (vehicle == null)
            {
                return NotFound();
            }
            return View(vehicle);
        }

        // POST: Vehicles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("RegNr,Model,Color,VehicleType,CheckInVehicle")] Vehicle vehicle)
        {
            if (id != vehicle.RegNr)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vehicle);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VehicleExists(vehicle.RegNr))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(vehicle);
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicle = await _context.Vehicle
                .FirstOrDefaultAsync(m => m.RegNr == id);
            if (vehicle == null)
            {
                return NotFound();
            }

            return View(vehicle);
        }


        //RÖÖR EJ! 
        // POST: Vehicles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var vehicle = await _context.Vehicle.FindAsync(id);
            if (vehicle == null)
            {
                return NotFound();
            }

            var confirmationViewModel = new DeleteConfirmationViewModel
            {
                VehicleId = id,
                
            };
            return View("~/Views/Receipt/ConfirmReceipt.cshtml", confirmationViewModel); 
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        //RÖÖR EJ! 
        public IActionResult ConfirmReceipt(DeleteConfirmationViewModel viewModel)
        {
            var modelList = new List<DeleteConfirmationViewModel>();
            if (viewModel.ReceiptRequested)
            {
                var vehicle = _context.Vehicle.Find(viewModel.VehicleId);
                if (vehicle != null)
                {

                    var confirmationViewModel = new DeleteConfirmationViewModel
                    {
                         // Tilldela andra egenskaper från den hittade fordonet
                        VehicleId = viewModel.VehicleId,
                        RegNr = vehicle.RegNr,
                        Model = vehicle.Model,
                        Color = vehicle.Color,
                        CheckInVehicle = vehicle.CheckInVehicle,
                        VehicleType = vehicle.VehicleType.ToString(),
                    };

                    TimeSpan timeDifference = DateTime.Now - vehicle.CheckInVehicle;
                    confirmationViewModel.CheckInTimeDifference = timeDifference;
                    modelList.Add(confirmationViewModel);


                    _context.Vehicle.Remove(vehicle);
                    _context.SaveChanges();
                    // Lägg till det nya objektet i listan
                 
                }

                // Skicka användaren till Receipt-sidan med samma värden

                return View("~/Views/Receipt/Receipt.cshtml", modelList);
            }
            else
            {
                var vehicle = _context.Vehicle.Find(viewModel.VehicleId);
                if (vehicle != null)
                {
                    _context.Vehicle.Remove(vehicle);
                    _context.SaveChanges();
                }
                // Om kvitto inte begärs, gör något annat eller omdirigera någon annanstans
                return RedirectToAction(nameof(Index));
            }
        }




        private bool VehicleExists(string id)
        {
            return _context.Vehicle.Any(e => e.RegNr == id);
        }

        [HttpGet]
        public async Task<IActionResult> Search(string searchString)
        {
            
            if (string.IsNullOrEmpty(searchString))
            {
                var vehicleContext = await _context.Vehicle.ToListAsync();
                ViewBag.FilterWasSuccess = null;
                return View("Index", vehicleContext);
            }

            var vehicles = await _context.Vehicle
                    .Where(vehicle =>
                        vehicle.RegNr.Contains(searchString) ||
                        vehicle.Model.Contains(searchString) ||
                        vehicle.Color.Contains(searchString)
                    )
                    .ToListAsync();

            if (vehicles.Count == 0)
            {
                ViewBag.FilterWasSuccess = false;
                ViewBag.Message = "No vehicles were found 🤷";
                var vehicleContext = await _context.Vehicle.ToListAsync();
                return View("Index", vehicleContext);
            }
            ViewBag.FilterWasSuccess = true;
            ViewBag.Message = $"Found {vehicles.Count} vehicle{(vehicles.Count > 1 ? "s" : "")} 🚗";
            return View("Index", vehicles);

        }
    }
}
