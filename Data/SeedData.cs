using ConstellationWebApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace ConstellationWebApp.Data
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ConstellationWebAppContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<ConstellationWebAppContext>>()))
            {
                // Look for any movies.
                if (context.User.Any())
                {
                    return;   // DB has been seeded
                }

                context.User.AddRange(

                     new User { UserName = "alcar123",  Password = "Carson1", FirstName = "Carson",  LastName = "Alexander",Bio = "history major",       Seeking = "Non-Paid Internship",              ImageSource = "", ContactLinkOne = "23ALEX@FAKE.gmail.com"       },
                     new User { UserName = "Roland123", Password = "Roland1", FirstName = "Roland",  LastName = "Nepay",    Bio = "New to the Program",  Seeking = "Paid Internship",                  ImageSource = "", ContactLinkOne = "789Roland@FAKE.gmail.com"    },
                     new User { UserName = "Dominic123",Password = "Dominic1",FirstName = "Dominic ",LastName = "Isiuwa",   Bio = "New to the Program",  Seeking = "Internship",                       ImageSource = "", ContactLinkOne = "789Dominic@FAKE.gmail.com"   },
                     new User { UserName = "Nwokolo123",Password = "Nwokolo1",FirstName = "Nwokolo ",LastName = "Ony",      Bio = "Looking for projects",Seeking = "Lead Developer",                   ImageSource = "", ContactLinkOne = "789Nwokolo@FAKE.gmail.com"   },
                     new User { UserName = "Jackson123",Password = "Jackson1",FirstName = "Jackson", LastName = "Dial",     Bio = "Stats Major",         Seeking = "Full Time Employment",             ImageSource = "", ContactLinkOne = "789Dial@FAKE.gmail.com"      },
                     new User { UserName = "Ryan123",   Password = "Ryan1",   FirstName = "Ryan ",   LastName = "Harris",   Bio = "Intern",              Seeking = "Part Time Employment",             ImageSource = "", ContactLinkOne = "789Ryan@FAKE.gmail.com"      },
                     new User { UserName = "David123",  Password = "David1",  FirstName = "David ",  LastName = "Miller",   Bio = "Teacher",             Seeking = "Aid students in thier Development",ImageSource = "", ContactLinkOne = "789Miller@FAKE.gmail.com"    },
                     new User { UserName = "Kory123",   Password = "Kory1",   FirstName = "Kory ",   LastName = "Godfrey",  Bio = "Teacher",             Seeking = "Aid students in thier Development",ImageSource = "", ContactLinkOne = "789KoryG@FAKE.gmail.com"     },
                     new User { UserName = "Sean123",   Password = "Sean1",   FirstName = "Sean ",   LastName = "Murdock",  Bio = "Teacher",             Seeking = "Aid students in thier Development",ImageSource = "", ContactLinkOne = "789SMurdock@FAKE.gmail.com"  }
                );
                context.SaveChanges();

                context.Project.AddRange(
                     new Project { Title = "alcar-Project1", Description = "Javascript App", StartDate = new DateTime(2008, 16, 2), ProjectLinkOne = "23ALEX@FAKE.gmail.com"      },
                     new Project { Title = "alcar-Project2", Description = "Javascript App", StartDate = new DateTime(2018, 16, 2), ProjectLinkOne = "23ALEX@FAKE.gmail.com"      },
                     new Project { Title = "alcar-Project3", Description = "Javascript App", StartDate = new DateTime(2020, 16, 2), ProjectLinkOne = "23ALEX@FAKE.gmail.com"      },
                     new Project { Title = "Roland-Project1", Description = "Webpage",       StartDate = new DateTime(2008, 16, 2), ProjectLinkOne = "789Roland@FAKE.gmail.com"   }, 
                     new Project { Title = "Dominic-Project1", Description = "Database",     StartDate = new DateTime(2018, 16, 2), ProjectLinkOne = "789Dominic@FAKE.gmail.com"  },
                     new Project { Title = "Nwokolo-Project1", Description = "Resume Site",  StartDate = new DateTime(2020, 16, 2), ProjectLinkOne = "789Nwokolo@FAKE.gmail.com"  },
                     new Project { Title = "Jackson-Project1", Description = "Spreadsheet",  StartDate = new DateTime(2008, 16, 2), ProjectLinkOne = "789Dominic@FAKE.gmail.com"  },
                     new Project { Title = "Ryan-Project1", Description = "Constellation",   StartDate = new DateTime(2018, 16, 2), ProjectLinkOne = "789Nwokolo@FAKE.gmail.com"  },
                     new Project { Title = "David-Project1", Description = "225 Class",      StartDate = new DateTime(2020, 16, 2), ProjectLinkOne = "789Dial@FAKE.gmail.com"     },
                     new Project { Title = "Kory-Project1", Description = "380 Class",       StartDate = new DateTime(2008, 16, 2), ProjectLinkOne = "789Ryan@FAKE.gmail.com"     },
                     new Project { Title = "SEAN-Project1", Description = "262 Class",       StartDate = new DateTime(2008, 16, 2), ProjectLinkOne = "789SMurdock@FAKE.gmail.com" }
                );                                                                                                                                                                
                context.SaveChanges();                                                                                                                                            
            }
        }
    }
}       