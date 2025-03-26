using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GYM_Manage.Models
{
    public class LichTap
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MaLichTap { get; set; }

        [ForeignKey("HuanLuyenVien")]
        public int MaHuanLuyenVien { get; set; }
        public HuanLuyenVien HuanLuyenVien { get; set; }

        [ForeignKey("ThanhVien")]
        public int MaThanhVien { get; set; }
        public ThanhVien ThanhVien { get; set; }

        [Required]
        public DateTime ThoiGianBatDau { get; set; }

        [Required]
        public DateTime ThoiGianKetThuc { get; set; }

        public string MoTa { get; set; }

        [Required]
        public string TrangThai { get; set; } = "DaDatLich";
    }
}
