using Contracts_and_Models.Models;
using Microsoft.EntityFrameworkCore;

namespace Property_and_Supply_Management.Database
{
	public class PAS_DBContext : DbContext
	{
		public PAS_DBContext(DbContextOptions <PAS_DBContext> options) : base(options)
		{
		}

		public DbSet<Department> Departments { get; set; }
		public DbSet<DisposedItem> DisposedItems { get; set; }
		public DbSet<Item> Items { get; set; }
		public DbSet<MaintenanceItem> MaintenanceItems { get; set; }
		public DbSet<ItemRequestRecords> itemRequestRecords { get; set; }
		public DbSet<EmergencyMedication> EmergencyMedications { get; set; }
		public DbSet<ExpiredMedicine> ExpiredMedicines { get; set; }
		public DbSet<DisposedMedication> DisposedMedications { get; set; }
        public DbSet<MedicationRequestRecords> MedicationRequestHistory { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			 modelBuilder.Entity<MedicationRequestRecords>()
				.HasOne(m => m.Medication)
				.WithMany()
				.HasForeignKey(m => m.medication_id)
				.OnDelete(DeleteBehavior.Restrict);

			modelBuilder.Entity<MedicationRequestRecords>()
				.HasOne(d => d.Department)
				.WithMany()
				.HasForeignKey(d => d.department_id)
				.OnDelete(DeleteBehavior.Restrict);

			modelBuilder.Entity<ItemRequestRecords>()
				.HasOne(d => d.Department)
				.WithMany()
				.HasForeignKey(d => d.department_id)
				.OnDelete(deleteBehavior: DeleteBehavior.Restrict);

			modelBuilder.Entity<ItemRequestRecords>()
				.HasOne(i => i.Item)
				.WithMany()
				.HasForeignKey(i => i.item_id)
				.OnDelete(DeleteBehavior.Restrict);


			modelBuilder.Entity<Department>().HasData(
				new Department { Id = 1,department_name = "Administration & Management Department", contact_person_email = ""},
				new Department { Id = 2, department_name = "Patient Services Department", contact_person_email = "" },
				new Department { Id = 3, department_name = "Medical & Clinical Department", contact_person_email = "" },
				new Department { Id = 4, department_name = "Pharmacy & Medication Management Department", contact_person_email = "" },
				new Department { Id = 5, department_name = "Laboratory & Diagnostics Department", contact_person_email = "" },
				new Department { Id = 6, department_name = "Front Desk & Customer Relations Department", contact_person_email = "" },
				new Department { Id = 7, department_name = "Finance & Accounting Department", contact_person_email = "" },
				new Department { Id = 8, department_name = "Human Resources Department", contact_person_email = "" },
				new Department { Id = 9, department_name = "Information Technology (IT) Department", contact_person_email = "" },
				new Department { Id = 10, department_name = "Engineering & Maintenance Department", contact_person_email = "" }
				);
		}
	}
}
