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
            //todo CheckInVehicle - automaticaly
            if (ModelState.IsValid)
            {
                vehicle.CheckInVehicle = DateTime.Now;
                if(string.IsNullOrWhiteSpace(vehicle.RegNr))
                {
                    ModelState.AddModelError("RegNr", "Registration number is required");
                    return View("Index", vehicle);
                }
                if (vehicle.RegNr.Contains("-"))
                {
                    ModelState.AddModelError("RegNr", "Registration number can not contain negative values");
                    return View("Index", vehicle);
                }
                _context.Add(vehicle);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View("Index", vehicle);
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
                RegNr = vehicle.RegNr,
                Model = vehicle.Model,
                Color = vehicle.Color,
                VehicleType = vehicle.VehicleType.ToString(), // Assuming VehicleType is an enum and you want to display its string representation
                CheckInVehicle = vehicle.CheckInVehicle
            };
            return View("~/Views/Receipt/ConfirmReceipt.cshtml", confirmationViewModel); // Skicka DeleteConfirmationViewModel till vyn
        }
        [HttpPost]
        [ValidateAntiForgeryToken]


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

                        // Tilldela andra egenskaper från den hittade bilen
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
                    // modelList.Add(confirmationViewModel);
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
    }
}
