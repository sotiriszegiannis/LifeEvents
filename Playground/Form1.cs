using Domain;
using Helper;
using Microsoft.EntityFrameworkCore;

namespace Playground
{
    public partial class Form1 : Form
    {
        public Form1()
        {

            //using (AppDbContext db = new AppDbContext)
            //{

            //    if (updatedEntity.Id == 0)
            //    {
            //        dbEntity = AddNew(db);
            //        updatedEntity.Id = dbEntity.Id;
            //    }
            //    else
            //    {
            //        dbEntity = GetDbSetForSave(db).Where(p => p.Id == updatedEntity.Id).FirstOrDefault();
            //    }
            //    Map(updatedEntity, dbEntity, db);
            //    if (dbEntity.GetType().IsSubclassOf(typeof(Tenant)))
            //        (dbEntity as Tenant).TenantId = TenantResolver.GetCurrentTenantId();
            //    OnBeforeSave(dbEntity, db);
            //    db.SaveChanges();
            //    OnAfterSave(dbEntity, db);
            //}
            var timeNowInAthens=DateTime.UtcNow.ToIanaDate("Europe/Athens");
            var info=TimeZoneInfo.FindSystemTimeZoneById("Europe/Athens");
            InitializeComponent();
        }
    }

    public class Parent
    {
        public IChild AChild { get; set; }
        public void Save()
        {
            AChild.Save();
        }
    }
    public interface IChild
    {
        bool Save();
    }
    public class Child:IChild
    {
        public bool Save()
        {
            Console.WriteLine("saved");
            return true;
        }
    }
}