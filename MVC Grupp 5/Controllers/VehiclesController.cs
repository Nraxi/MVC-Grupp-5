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

        // GET: Vehicles/Delete/5
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
            TimeSpan formattedParkedTime = DateTime.Now - vehicle.CheckInVehicle;
            int roundedSeconds = (int)Math.Round(formattedParkedTime.TotalSeconds);
            formattedParkedTime = TimeSpan.FromSeconds(roundedSeconds);
            ViewBag.ParkedTime = formattedParkedTime.ToString();
            return View(vehicle);
        }

        // POST: Vehicles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var vehicle = await _context.Vehicle.FindAsync(id);
            if (vehicle != null)
            {
                _context.Vehicle.Remove(vehicle);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VehicleExists(string id)
        {
            return _context.Vehicle.Any(e => e.RegNr == id);
        }
    }
}
