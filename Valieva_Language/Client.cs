//Client.cs
namespace Valieva_Language
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public partial class Client
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Client()
        {
            this.ClientService = new HashSet<ClientService>();
            this.Tag = new HashSet<Tag>();
        }

        public int ID { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Patronymic { get; set; }
        public int GenderCode { get; set; }
        public string Phone { get; set; }
        public string PhotoPath { get; set; }
        public System.DateTime Birthday { get; set; }

        public string BirthdayString
        {
            get
            {
                if (Birthday != null)
                    return Birthday.ToShortDateString();
                else return "";
            }
            set
            {
                try
                {
                    Birthday = DateTime.Parse(value);
                }
                catch
                {
                    Birthday = DateTime.Parse("01.02.2003");
                }
            }
        }

        public string Email { get; set; }
        public System.DateTime RegistrationDate { get; set; }
        public string RegistrationDateString
        {
            get
            {
                if (RegistrationDate != null)
                    return RegistrationDate.ToShortDateString();
                else return "";
            }
        }

        public virtual Gender Gender { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ClientService> ClientService { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tag> Tag { get; set; }

        public string LastJoin
        {
            get
            {
                var LastJoinItem = ValievaLanguageEntities.GetContext().ClientService.Where(p => (p.ClientID == this.ID)).OrderByDescending(p => p.StartTime);

                if (LastJoinItem.Count() != 0)
                {
                    return LastJoinItem.Distinct().First().StartTime.ToShortDateString();
                }
                else
                {
                    return "нет";
                }
            }
        }

        public int countOfJoinsc
        {
            get
            {
                try
                {
                    var LastJoinItem = ValievaLanguageEntities.GetContext().ClientService.Where(p => (p.ClientID == this.ID)).ToList();

                    return LastJoinItem.Count();
                }
                catch (Exception ex)
                {
                    return 0;
                }
            }

        }
    }
}