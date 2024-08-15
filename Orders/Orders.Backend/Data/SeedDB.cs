
using Orders.Shared.Entities;

namespace Orders.Backend.Data
{
    public class SeedDB
    {
        private readonly DataContext _context;

        public SeedDB(DataContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();
            await CheckCountriesAsync();
            await CheckCategoriesAsync();


        }

        private async Task CheckCategoriesAsync()
        {
            if (!_context.Categories.Any())
            {
                _context.Categories.Add(new Shared.Entities.Category { Name = "Tecnologia" });
                _context.Categories.Add(new Shared.Entities.Category { Name = "Hogar" });
                _context.Categories.Add(new Shared.Entities.Category { Name = "Telefonia" });
                await _context.SaveChangesAsync();

            }
        }

        private async Task CheckCountriesAsync()
        {
            if (!_context.Countries.Any()) {
                _context.Countries.Add(new Country
                {
                    Name = "Ecuador",
                    States = new List<State>() {
                        new ()
                        {
                            Name = "Loja",
                            Cities = new List<City>()
                            {
                                new () { Name = "Loja" },
                                new () { Name = "Paltas" },
                                new () { Name = "Catamayo" }
                            }
                        },
                        new ()
                        {
                            Name = "Guayaquil",
                            Cities = new List<City>()
                            {
                                new () { Name = "Durán" },
                                new () { Name = "Samborondom" },
                                new () { Name = "Nobol" }
                            }
                        }


                    }
                });

                _context.Countries.Add(new Country
                {
                    Name = "Colombia",
                    States = new List<State>() {
                        new ()
                        {
                            Name = "Antioquia",
                            Cities = [
                            
                                new () { Name = "Medellín" },
                                new () { Name = "Itagui" },
                                new () { Name = "Envigado" }
                            ]
                        },
                        new ()
                        {
                            Name = "Bogota",
                            Cities = [
                            
                                new () { Name = "Usaquen" },
                                new () { Name = "Santa Fé" },
                                new () { Name = "Champinero" }
                            ]
                        }


                    }
                });

                _context.Countries.Add(new Country
                {
                    Name = "Estados Unidos",
                    States = new List<State>() {
                        new ()
                        {
                            Name = "Florida",
                            Cities = [

                                new () { Name = "Orlando" },
                                new () { Name = "Miami" },
                                new () { Name = "Tampa" }
                            ]
                        },
                        new ()
                        {
                            Name = "Texas",
                            Cities = [

                                new () { Name = "Houston" },
                                new () { Name = "Dallas" },
                                new () { Name = "San Antonio" }
                            ]
                        }


                    }
                });

                await _context.SaveChangesAsync();

            }
        }
    }
}
