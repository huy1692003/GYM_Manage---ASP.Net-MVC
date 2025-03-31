using GYM_Manage.Data;
using GYM_Manage.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GYM_Manage.Controllers.Admin
{
    [Area("Admin")]
    public class QL_HuanLuyenVienController : Controller
    {
        private readonly GYM_DBcontext _context;

        public QL_HuanLuyenVienController(GYM_DBcontext context)
        {
            _context = context;
        }

        // GET: Admin/HuanLuyenVien
        public async Task<IActionResult> Index()
        {
            // Check if there are any trainers in the database
            if (!await _context.HuanLuyenViens.AnyAsync())
            {
                
                // Create 10 sample trainers if none exist
                var sampleHuanLuyenViens = new List<HuanLuyenVien>
                {
                    new HuanLuyenVien { HoTen = "Nguyễn Văn Anh", ChuyenMon = "Yoga", ChungChi = "Chứng chỉ Yoga quốc tế", NgayTuyenDung = DateTime.Now.AddYears(-2) },
                    new HuanLuyenVien { HoTen = "Trần Thị Bình", ChuyenMon = "Thể hình", ChungChi = "ISSA Personal Trainer", NgayTuyenDung = DateTime.Now.AddYears(-1) },
                    new HuanLuyenVien { HoTen = "Lê Văn Cường", ChuyenMon = "Cardio", ChungChi = "ACE Certified Personal Trainer", NgayTuyenDung = DateTime.Now.AddMonths(-8) },
                    new HuanLuyenVien { HoTen = "Phạm Thị Dung", ChuyenMon = "Pilates", ChungChi = "Chứng chỉ Pilates quốc tế", NgayTuyenDung = DateTime.Now.AddMonths(-10) },
                    new HuanLuyenVien { HoTen = "Hoàng Văn Em", ChuyenMon = "CrossFit", ChungChi = "CrossFit Level 2 Trainer", NgayTuyenDung = DateTime.Now.AddYears(-3) },
                    new HuanLuyenVien { HoTen = "Ngô Thị Fun", ChuyenMon = "Boxing", ChungChi = "USA Boxing Coach", NgayTuyenDung = DateTime.Now.AddMonths(-6) },
                    new HuanLuyenVien { HoTen = "Đỗ Văn Giang", ChuyenMon = "Dinh dưỡng thể thao", ChungChi = "NASM Fitness Nutrition Specialist", NgayTuyenDung = DateTime.Now.AddYears(-1).AddMonths(-3) },
                    new HuanLuyenVien { HoTen = "Vũ Thị Hằng", ChuyenMon = "Phục hồi chấn thương", ChungChi = "NASM Corrective Exercise Specialist", NgayTuyenDung = DateTime.Now.AddMonths(-9) },
                    new HuanLuyenVien { HoTen = "Đinh Văn Inh", ChuyenMon = "Thể dục nhịp điệu", ChungChi = "ACE Group Fitness Instructor", NgayTuyenDung = DateTime.Now.AddYears(-2).AddMonths(-5) },
                    new HuanLuyenVien { HoTen = "Bùi Thị Khiêm", ChuyenMon = "Powerlifting", ChungChi = "IPF Coach Level 1", NgayTuyenDung = DateTime.Now.AddMonths(-4) }
                };

                await _context.HuanLuyenViens.AddRangeAsync(sampleHuanLuyenViens);
                await _context.SaveChangesAsync();
            }

            var huanLuyenViens = await _context.HuanLuyenViens.Include(h => h.NguoiDung).ToListAsync();
            return View("~/Views/Admin/QL_HuanLuyenVien/Index.cshtml", huanLuyenViens);
        }

        // GET: Admin/HuanLuyenVien/Search
        public async Task<IActionResult> Search(string keyword)
        {
            var huanLuyenViens = await _context.HuanLuyenViens
                .Include(h => h.NguoiDung)
                .Where(h => h.NguoiDung.HoTen.Contains(keyword) || h.ChuyenMon.Contains(keyword))
                .ToListAsync();

            if (huanLuyenViens.Count == 0)
            {
                TempData["ErrorMessage"] = "Chúng tôi không tìm thấy huấn luyện viên nào phù hợp!";
                return RedirectToAction(nameof(Index));
            }
            
            TempData["SuccessMessage"] = $"Chúng tôi tìm thấy {huanLuyenViens.Count} huấn luyện viên phù hợp!";
            return View("~/Views/Admin/QL_HuanLuyenVien/Index.cshtml", huanLuyenViens);
        }

        // GET: Admin/HuanLuyenVien/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var huanLuyenVien = await _context.HuanLuyenViens
                .Include(h => h.NguoiDung)
                .FirstOrDefaultAsync(m => m.MaHuanLuyenVien == id);
                
            if (huanLuyenVien == null)
            {
                return NotFound();
            }

            return View(huanLuyenVien);
        }

        // GET: Admin/HuanLuyenVien/Create
        public IActionResult Create()
        {
            return View("~/Views/Admin/QL_HuanLuyenVien/CreateOrEdit.cshtml", new HuanLuyenVien());
        }

        // POST: Admin/HuanLuyenVien/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("HoTen,MaNguoiDung,ChuyenMon,ChungChi,NgayTuyenDung")] HuanLuyenVien huanLuyenVien)
        {
            if (ModelState.IsValid)
            {
                _context.Add(huanLuyenVien);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Thêm thành công!";
                return RedirectToAction(nameof(Index));
            }
            TempData["ErrorMessage"] = "Thêm thất bại!";
            return View("~/Views/Admin/QL_HuanLuyenVien/CreateOrEdit.cshtml");
        }

        // GET: Admin/HuanLuyenVien/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var huanLuyenVien = await _context.HuanLuyenViens.FindAsync(id);
            if (huanLuyenVien == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy huấn luyện viên nào phù hợp!";
                return RedirectToAction(nameof(Index));
            }
            return View("~/Views/Admin/QL_HuanLuyenVien/CreateOrEdit.cshtml", huanLuyenVien);
        }

        // POST: Admin/HuanLuyenVien/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("HoTen,MaHuanLuyenVien,MaNguoiDung,ChuyenMon,ChungChi,NgayTuyenDung")] HuanLuyenVien huanLuyenVien)
        {
            if (id != huanLuyenVien.MaHuanLuyenVien)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(huanLuyenVien);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Chỉnh sửa thành công!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HuanLuyenVienExists(huanLuyenVien.MaHuanLuyenVien))
                    {
                        TempData["ErrorMessage"] = "Không tìm thấy huấn luyện viên nào phù hợp!";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View("~/Views/Admin/QL_HuanLuyenVien/CreateOrEdit.cshtml", huanLuyenVien);
        }

        // GET: Admin/HuanLuyenVien/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var huanLuyenVien = await _context.HuanLuyenViens.FindAsync(id);
            if (huanLuyenVien != null)
            {
                _context.HuanLuyenViens.Remove(huanLuyenVien);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Xóa thành công!";
            }
            else
            {
                TempData["ErrorMessage"] = "Xóa thất bại không tìm thấy huấn luyện viên nào phù hợp!";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool HuanLuyenVienExists(int id)
        {
            return _context.HuanLuyenViens.Any(e => e.MaHuanLuyenVien == id);
        }
    }
}
