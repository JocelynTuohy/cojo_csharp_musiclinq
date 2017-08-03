using System;
using System.Collections.Generic;
using System.Linq;
using JsonData;

namespace ConsoleApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //Collections to work with
            List<Artist> Artists = JsonToFile<Artist>.ReadJson();
            List<Group> Groups = JsonToFile<Group>.ReadJson();

            //========================================================
            //Solve all of the prompts below using various LINQ queries
            //========================================================

            //There is only one artist in this collection from Mount Vernon, what is their name and age?
            Artist fromVernon = Artists.Where(artist => artist.Hometown == "Mount Vernon").Single();
            // Console.WriteLine(Artists.Where(artist => artist.Hometown == "Mount Vernon"));
            Console.WriteLine(fromVernon.RealName + " is " + fromVernon.Age + " years old");

            //Who is the youngest artist in our collection of artists?
            Artist youngest = Artists.OrderBy(artist => artist.Age).First();
            Console.WriteLine("{0} is the youngest artist.", youngest.RealName);

            //Display all artists with 'William' somewhere in their real name
            List<Artist> billArtists = Artists.Where(artist => artist.RealName.Contains("William")).ToList();
            foreach (Artist dude in billArtists)
            {
                Console.WriteLine("{0} is a William dude.", dude.RealName);
            }

            // Display all groups with names less than 8 characters in length.
            List<Group> shortGroups = Groups.Where(group => group.GroupName.Length < 8).ToList();
            foreach (Group shortie in shortGroups)
            {
                Console.WriteLine("{0} is one of the short-named groups.", shortie.GroupName);
            }

            //Display the 3 oldest artist from Atlanta
            List<Artist> oldAtlantas = Artists.Where(artist => artist.Hometown == "Atlanta").OrderByDescending(artist => artist.Age).Take(3).ToList();
            foreach (Artist fart in oldAtlantas)
            {
                Console.WriteLine("{0} is {1} years old.", fart.RealName, fart.Age);
            }

            //(Optional) Display the Group Name of all groups that have at least one member not from New York City
            List<Artist> outsiderArtists = Artists.Where(artist => artist.Hometown != "New York City").ToList();
            List<Group> outsiderGroups = Groups.Join(
                outsiderArtists,
                group => group.Id,
                artist => artist.GroupId,
                (group, artist) =>
                {
                    return group;
                }
            ).Distinct().ToList();
            // Console.WriteLine(outsiderGroups);
            foreach (Group thisgroup in outsiderGroups)
            {
                Console.WriteLine("{0} is one of the groups with at least one non-NYC member.", thisgroup.GroupName);
            }
            //(Optional) Display the artist names of all members of the group 'Wu-Tang Clan'
            int wutangid = Groups.Where(group => group.GroupName == "Wu-Tang Clan").Single().Id;
            List<Artist> wutangmembers = Artists.Where(artist => artist.GroupId == wutangid).ToList();
            foreach (Artist clanner in wutangmembers)
            {
                Console.WriteLine("{0} is a member of Wu-Tang Clan.", clanner.ArtistName);
            }


            // OFFICIAL SOLUTION
            Console.WriteLine("Official Solution");
            List<string> NonNewYorkGroups = Artists
                                .Join(Groups, artist => artist.GroupId, group => group.Id, (artist, group) => { artist.Group = group; return artist;})
                                .Where(artist => (artist.Hometown != "New York City" && artist.Group != null))
                                .Select(artist => artist.Group.GroupName)
                                .Distinct()
                                .ToList();
            Console.WriteLine("All groups with members not from New York City:");
            foreach(var group in NonNewYorkGroups){
                Console.WriteLine(group);
            }
        }
    }
}
