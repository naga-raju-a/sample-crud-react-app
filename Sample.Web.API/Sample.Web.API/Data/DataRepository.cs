using Sample.Web.API.Models;
using Sample.Web.API.Services;

namespace Sample.Web.API.Data
{
    public class DataRepository
    {
        public static void SeedData(AppDbContext _context)
        {
            if (!_context.Cafe.Any())
            {
                _context.Cafe.AddRange(new List<Cafe>
                {
                    new Cafe {
                        Id = Guid.NewGuid(),
                        Name = "Cafe Mocha",
                        Location = "Singapore",
                        Description = "A cozy cafe serving coffee and pastries in a relaxing ambiance.",
                        Logo = "https://th.bing.com/th/id/OIP.GWKrFKagojjcEaiRsjoIggAAAA?rs=1&pid=ImgDetMain&cb=idpwebpc2"
                    },
                    new Cafe {
                        Id = Guid.NewGuid(),
                        Name = "Tiong Bahru Bakery",
                        Location = "Singapore",
                        Description = "Famous for its artisanal French pastries and fresh bakes.",
                        Logo = ""
                    },
                    new Cafe {
                        Id = Guid.NewGuid(),
                        Name = "Common Man Coffee Roasters",
                        Location = "Singapore",
                        Description = "Specialty coffee roaster with brunch and quality beans sourced worldwide.",
                        Logo = "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAACAAAAAgCAMAAABEpIrGAAAAZlBMVEVVeFZXe1iSqIWdsY5nh2SnuJWLo4CFnnt4lHFVeldxkG1+l3b//+jZ3bv49NJbgFzk5cT5+drp6cfw7sxXfVmitZLz89P//t3g48D//+HK07B/m3e1w6BReFTN1bK6x6T//+xrjGgH92QmAAABAUlEQVR4AZ3TBW4sIQAA0Mfo2te63v9cdffVsSbEGi3pKPJwCIQvd4MqhlbGgrnvwVirFnQKBG/+CJZquaBNAME/dMpYbab0qjbxYaT0mAgqrQbARu+/4N7/H4I59tzb1UgHpaU3dezkBAOGRDBWmLkTbBs8+quPTTJKBsFfhQfbrk2Upq78MdfbMk8AQe7IEEFrjqnb2PHCnZAAxnYFrRuHnuw4t6N0b2PX0msS+KMR8KEzMbelttDItUIyaFEaW3qJ4Mpvv10q0gHm9nyYR/CqUHjUpQNlHOTEeQSDW5W1JgFQigdHpfZoavDPnZVTz0IKiJ/cYJBrUMktkcs034NPYOfE6/22cbEAAAAASUVORK5CYII="
                    },
                    new Cafe {
                        Id = Guid.NewGuid(),
                        Name = "Atlas Coffeehouse",
                        Location = "Singapore",
                        Description = "Trendy cafe with rich coffee and fusion brunch dishes.",
                        Logo = ""
                    },
                    new Cafe {
                        Id = Guid.NewGuid(),
                        Name = "The Populus Coffee & Food Co.",
                        Location = "Singapore",
                        Description = "Modern cafe with specialty coffee and innovative food menu.",
                        Logo = ""
                    }
                });
                _context.SaveChanges();
            }

            if (!_context.Employee.Any())
            {
               
                _context.Employee.AddRange(new List<Employee>
                {
                    new Employee {
                        Id = GenerateEmployeeId(),
                        Name = "Teck Wu",
                        EmailAddress ="teck.wu@cafemocha.com",
                        PhoneNumber = "83456789",
                        Gender = "Male",
                        CafeId = _context.Cafe.First().Id.ToString(),
                        EmploymentDate = new DateTime(2024, 5, 05),
                    },
                    new Employee {
                        Id = GenerateEmployeeId(),
                        Name = "Roy Tan",
                        EmailAddress = "roy.tan@example.com",
                        PhoneNumber = "83456789",
                        Gender = "Male",
                        CafeId = _context.Cafe.Last().Id.ToString(),
                        EmploymentDate = new DateTime(2024, 3, 21),
                    },
                    new Employee {
                        Id = GenerateEmployeeId(),
                       Name = "Ava Lee",
                        EmailAddress = "ava.lee@example.com",
                        PhoneNumber = "82345678",
                        Gender = "Female",
                        CafeId = _context.Cafe.First().Id.ToString(),
                        EmploymentDate = new DateTime(2024, 1, 10),
                    },
                });

                _context.SaveChanges();
            }

            
        }

        public static string GenerateEmployeeId()
        {
            var rnd = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            string id;
            do
            {
                id = "UI" + new string(Enumerable.Range(0, 7).Select(_ => chars[rnd.Next(chars.Length)]).ToArray());
            } while (!id.Skip(2).Any(char.IsDigit)); // Ensure at least one digit
            return id;
        }
    }


}
