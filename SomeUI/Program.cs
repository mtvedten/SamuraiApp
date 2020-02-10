using System;
using SamuraiApp.Domain;
using SamuraiApp.Data;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SomeUI
{
    class Program
    {
        private static SamuraiContext _context = new SamuraiContext();
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            //   InsertSamurai();
            // InsertMultipleSamurai();
            //SimpleSamuraiQyery();
            //MoreQuieries();
            //RetriveAndUpdateSamurai();
            //RetriveAndUpdateMultipleSamurai();
            //InsertBattle();
            //QueryAndUpdateBattle_Disconneted();
            //DeleteWhileTraked();
            //InsertNewPkFkGraph();
            //AddChildToExistingObjectsWhileTracked();
            //AddChildToExistingObjectsWhileNotTracked(2);
            //EagerLoadSamuraiWithQutes();

            // var dynamicList = ProjectDynamic();

            //FliterWithRelatedData();
            ModifyingRelatedDataWhenNotTracked();

            Console.ReadLine();
        }



        private static void InsertSamurai()
        {
            var samurai = new Samurai { Name = "Morten" };
            using (var context = new SamuraiContext())
            {
                context.Samurais.Add(samurai);
                context.SaveChanges();
                Console.WriteLine("Samurai saved");

            }
        }

        private static void InsertMultipleSamurai()
        {
            var samurai = new Samurai { Name = "Buster XXL" };
            var samurai2 = new Samurai { Name = "Flipper" };

            using (var context = new SamuraiContext())
            {
                context.Samurais.AddRange(samurai, samurai2);
                context.SaveChanges();
                Console.WriteLine("Multiple Samurai saved");

            }
        }

        private static void SimpleSamuraiQyery()
        {
            using (var context = new SamuraiContext())
            {
                var samurais = context.Samurais.ToList();
            }
        }

        private static void MoreQuieries()
        {
            var name = "Buster";
            // var samurais = _context.Samurais.FirstOrDefault(s => s.Name == name);
            var samurais = _context.Samurais.Find(3);
        }

        private static void RetriveAndUpdateSamurai()
        {
            var samurai = _context.Samurais.FirstOrDefault();
            samurai.Name += " San";
            _context.SaveChanges();
        }

        private static void RetriveAndUpdateMultipleSamurai()
        {
            var samurais = _context.Samurais.ToList();
            samurais.ForEach(s => s.Name += " San");
            _context.SaveChanges();
        }

        private static void InsertBattle()
        {
            _context.Battles.Add(new Battle { Name = "Battle of Okehazama", StartDate = new DateTime(1560, 05, 01), EndDate = new DateTime(1560, 06, 15) });
            _context.SaveChanges();
        }

        private static void QueryAndUpdateBattle_Disconneted()
        {
            var battle = _context.Battles.FirstOrDefault();
            battle.EndDate = new DateTime(1560, 06, 30);

            using (var newContextInstance = new SamuraiContext())
            {
                newContextInstance.Battles.Update(battle);
                newContextInstance.SaveChanges();
            }
        }

        private static void DeleteWhileTraked()
        {
            var samurai = _context.Samurais.FirstOrDefault(s => s.Name == "Flipper San");
            _context.Samurais.Remove(samurai);
            _context.SaveChanges();
        }

        private static void InsertNewPkFkGraph()
        {
            var samurai = new Samurai
            {
                Name = "Ken Kadivara",
                Quotes = new List<Quote>
                {
                    new Quote {Text = "I've come to sav you"},
                    new Quote {Text = "I was here"}
                }
            };
            _context.Samurais.Add(samurai);
            _context.SaveChanges();
        }

        private static void AddChildToExistingObjectsWhileTracked()
        {
            var samurai = _context.Samurais.First();
            samurai.Quotes.Add(new Quote { Text = "I bet you are happy i saved you to!" });
            _context.SaveChanges();
        }

        private static void AddChildToExistingObjectsWhileNotTracked(int samuraiId)
        {
            var quote = new Quote { Text = "Ship o hoi!", SamuraiId = samuraiId };

            using (var newContext = new SamuraiContext())
            {
                newContext.Quotes.Add(quote);
                newContext.SaveChanges();
            }


        }

        // Query related data

        private static void EagerLoadSamuraiWithQutes()
        {
            var samuraiWithQuotes = _context.Samurais.Include(s => s.Quotes).ToList();
            foreach (var samurai in samuraiWithQuotes) {
                Console.WriteLine(samurai.Name);

                foreach (var quote in samurai.Quotes)
                {
                    Console.WriteLine(" - {0}", quote.Text);
                }
            }
        }

        private static List<dynamic> ProjectDynamic() {
            var someProperties = _context.Samurais.Select(s => new { s.Id, s.Name }).ToList();
            return someProperties.ToList<dynamic>();
            }


        private static void FliterWithRelatedData()
        {
            var samurais = _context.Samurais
                .Where(s => s.Quotes.Any(q => q.Text.Contains("bet")))
                .ToList();
        }

        // Modify related data

        private static void ModifyingRelatedDataWhenTracked()
        {
            var samurai = _context.Samurais.Include(s => s.Quotes).FirstOrDefault();
            samurai.Quotes[0].Text += "Did you hear that";
            _context.SaveChanges();
        }

        private static void ModifyingRelatedDataWhenNotTracked()
        {
            var samurai = _context.Samurais.Include(s => s.Quotes).FirstOrDefault();
            var quote = samurai.Quotes[0];
            quote.Text = "Yes, I did it aiagn!";
            
            using (var newContext = new SamuraiContext())
            {
                //newContext.Quotes.Update(quote);
                newContext.Entry(quote).State = EntityState.Modified;
                newContext.SaveChanges();
            }
         
        }
    }
}
