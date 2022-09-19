using Prowler.Presentation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Prowler.Presentation.Helpers
{
    public static class MockHelper
    {
        public static MockProduct GetMockProducts(bool attachPleaseSelectItem = true)
        {
            var mockProduct = new MockProduct()
            {
                ProductDataSource = new List<Product>()
            };

            if (attachPleaseSelectItem)
            {
                mockProduct.ProductDataSource.Add(new Product
                {
                    Id = -1,
                    Name = "Please select",
                    GroupId = -1,
                    Image = "/Content/Images/shop.jpg",
                    Description = "a product"
                });
            }

            mockProduct.ProductDataSource.Add(new Product
            {
                Id = 1,
                Name = "Shaorma",
                GroupId = 0,
                GroupName = "Fast food",
                Image = "/Content/Images/shaorma.jpg",
                Description = "Lipie, Meat, french fries, cabbage salad, garlic sauce, tzatziki sauce"
            });
            mockProduct.ProductDataSource.Add( new Product
            {
                Id = 2,
                Name = "Kebab",
                GroupId = 0,
                GroupName = "Fast food",
                Image = "/Content/Images/kebab.jpg",
                Description = "Meat, french fries, cabbage salad, garlic sauce, tzatziki sauce"
            });
            mockProduct.ProductDataSource.Add( new Product
            {
                Id = 3,
                Name = "Hamburger beef or chicken",
                GroupId = 0,
                GroupName = "Fast food",
                Image = "/Content/Images/hamburger.jpg",
                Description = "Meatballs, potatoes, cabbage with mayonnaise, tartar sauce, garlic sauce"
            });
            mockProduct.ProductDataSource.Add( new Product
            {
                Id = 4,
                Name = "Cheeseburger beef or chicken",
                GroupId = 0,
                GroupName = "Fast food",
                Image = "/Content/Images/cheeseburger.png",
                Description = "Meatballs, cheese, french fries, egg, cabbage with mayonnaise, tartar sauce, garlic"
            });
            mockProduct.ProductDataSource.Add( new Product
            {
                Id = 5,
                Name = "Eggburger",
                GroupId = 0,
                GroupName = "Fast food",
                Image = "/Content/Images/hamburger.jpg",
                Description = "Egg, french fries, cabbage with mayonnaise, tartar sauce, garlic sauce"
            });
            mockProduct.ProductDataSource.Add( new Product
            {
                Id = 6,
                Name = "Hot-Dog with sausages",
                GroupId = 0,
                GroupName = "Fast food",
                Image = "/Content/Images/hotdog.png",
                Description = "Sausages, french fries, cabbage with mayonnaise, tartar sauce, garlic sauce"
            });
            mockProduct.ProductDataSource.Add( new Product
            {
                Id = 7,
                Name = "Bocadillo with ham",
                GroupId = 0,
                GroupName = "Fast food",
                Image = "/Content/Images/bocadilo.jpg",
                Description = "Ham, cheese, peppers, tomatoes,cheese sauce"
            });
            mockProduct.ProductDataSource.Add( new Product
            {
                Id = 8,
                Name = "Ham toast",
                GroupId = 0,
                GroupName = "Fast food",
                Image = "/Content/Images/toste.png",
                Description = @"Sliced ​​bread (4), ham, cheese, spreadable cream"
            });

            mockProduct.ProductDataSource.Add( new Product
            {
                Id = 9,
                Name = "Long espresso",
                GroupId = 1,
                GroupName = "Beverage",
                Image = "/Content/Images/expreso.jpg",
                Description = "Coffee, sugar, milk"
            });
            mockProduct.ProductDataSource.Add( new Product
            {
                Id = 10,
                Name = "Red Bull",
                GroupId = 1,
                GroupName = "Beverage",
                Image = "/Content/Images/redbull.png",
                Description = "Energize"
            });
            mockProduct.ProductDataSource.Add( new Product
            {
                Id = 11,
                Name = "Mineral water",
                GroupId = 1,
                GroupName = "Beverage",
                Image = "/Content/Images/a_minerala.png",
                Description = "250ml"
            });
            mockProduct.ProductDataSource.Add( new Product
            {
                Id = 12,
                Name = "Mineral water carbonated",
                GroupId = 1,
                GroupName = "Beverage",
                Image = "/Content/Images/a_minerala.png",
                Description = "250ml"
            });
            mockProduct.ProductDataSource.Add( new Product
            {
                Id = 13,
                Name = "Coca Cola",
                GroupId = 1,
                GroupName = "Beverage",
                Image = "/Content/Images/cola.png",
                Description = "250ml"
            });
            mockProduct.ProductDataSource.Add( new Product
            {
                Id = 14,
                Name = "Exotic lemonade with oranges and lemon",
                GroupId = 1,
                GroupName = "Beverage",
                Image = "/Content/Images/limonada.jpg",
                Description = "500ml"
            });

            mockProduct.ProductDataSource.Add( new Product
            {
                Id = 15,
                Name = "Caesar salad with shrimp",
                GroupId = 2,
                GroupName = "Salads",
                Image = "/Content/Images/scaesar.png",
                Description = "Shrimp, iceberg lettuce, parmesan, Caesar dressing, croutons"
            });
            mockProduct.ProductDataSource.Add( new Product
            {
                Id = 16,
                Name = "Caesar salad",
                GroupId = 2,
                GroupName = "Salads",
                Image = "/Content/Images/scaesar.png",
                Description = "Chicken breast, iceberg lettuce, parmesan, Caesar dressing, croutons"
            });
            mockProduct.ProductDataSource.Add( new Product
            {
                Id = 17,
                Name = "Chicken salad",
                GroupId = 2,
                GroupName = "Salads",
                Image = "/Content/Images/spui.png",
                Description = "Chicken breast schnitzel, ice salad, peppers, tomatoes, carrots, croutons, mozzarella, dressing"
            });
            mockProduct.ProductDataSource.Add( new Product
            {
                Id = 18,
                Name = "Tuna salad",
                GroupId = 2,
                GroupName = "Salads",
                Image = "/Content/Images/s_ton.png",
                Description = "Tuna pieces, salad, onion, olives, corn, peppers, capers"
            });
            mockProduct.ProductDataSource.Add( new Product
            {
                Id = 19,
                Name = "Greek salad",
                GroupId = 2,
                GroupName = "Salads",
                Image = "/Content/Images/s_greceasca.png",
                Description = "Feta cheese, boiled egg, tomatoes, peppers, cucumbers, onions, ice salad, olives, dressing"
            });

            mockProduct.ProductDataSource.Add( new Product
            {
                Id = 20,
                Name = "Pizza Funghii",
                GroupId = 3,
                GroupName = "Pizza on fireplace",
                Image = "/Content/Images/pizza.png",
                Description = "mushrooms, cheese, mozzarella, pizza sauce"
            });
            mockProduct.ProductDataSource.Add( new Product
            {
                Id = 21,
                Name = "Pizza Vegetarian",
                GroupId = 3,
                GroupName = "Pizza on fireplace",
                Image = "/Content/Images/pizza.png",
                Description = "corn, tomatoes, peppers, onions, mushrooms, olives, cheese, mozzarella, pizza sauce"
            });
            mockProduct.ProductDataSource.Add( new Product
            {
                Id = 22,
                Name = "Pizza Sallami",
                GroupId = 3,
                GroupName = "Pizza on fireplace",
                Image = "/Content/Images/pizza.png",
                Description = "salami, mushrooms, cheese, mozzarella, pizza sauce"
            });
            mockProduct.ProductDataSource.Add( new Product
            {
                Id = 23,
                Name = "Pizza Prosciutto",
                GroupId = 3,
                GroupName = "Pizza on fireplace",
                Image = "/Content/Images/pizza.png",
                Description = "ham, cheese, mozzarella, pizza sauce"
            });
            mockProduct.ProductDataSource.Add( new Product
            {
                Id = 24,
                Name = "Pizza Quattro-Formaggi",
                GroupId = 3,
                GroupName = "Pizza on fireplace",
                Image = "/Content/Images/pizza.png",
                Description = "svaiter, cheese, mozzarella, gorgonzola, sour cream"
            });
            mockProduct.ProductDataSource.Add( new Product
            {
                Id = 25,
                Name = "Pizza Mexican",
                GroupId = 3,
                GroupName = "Pizza on fireplace",
                Image = "/Content/Images/pizza.png",
                Description = "ham, pork ham, spicy salami, peas, mushrooms, spicy sauce, mozzarella, cheese, pizza sauce"
            });
            mockProduct.ProductDataSource.Add( new Product
            {
                Id = 26,
                Name = "Pizza Quattro-Stagioni",
                GroupId = 3,
                GroupName = "Pizza on fireplace",
                Image = "/Content/Images/pizza.png",
                Description = "svaiter, cheese, mozzarella, gorgonzola, sour cream"
            });
            mockProduct.ProductDataSource.Add( new Product
            {
                Id = 27,
                Name = "Pizza Supreme",
                GroupId = 3,
                GroupName = "Pizza on fireplace",
                Image = "/Content/Images/pizza.png",
                Description = "salami, corn, kaizer, sausage, mushrooms, olives, cheese, mozzarella, pizza sauce"
            });

            mockProduct.ProductDataSource.Add( new Product
            {
                Id = 28,
                Name = "Tripe soup",
                GroupId = 4,
                GroupName = "Borsch",
                Image = "/Content/Images/ciorba.jpg",
                Description = "400ml"
            });
            mockProduct.ProductDataSource.Add( new Product
            {
                Id = 29,
                Name = "Meatball soup",
                GroupId = 4,
                GroupName = "Borsch",
                Image = "/Content/Images/ciorba.jpg",
                Description = "400ml"
            });
            mockProduct.ProductDataSource.Add( new Product
            {
                Id = 30,
                Name = "Greek soup",
                GroupId = 4,
                GroupName = "Borsch",
                Image = "/Content/Images/ciorba.jpg",
                Description = "400ml"
            });
            mockProduct.ProductDataSource.Add( new Product
            {
                Id = 31,
                Name = "Peasant soup with mushrooms",
                GroupId = 4,
                GroupName = "Borsch",
                Image = "/Content/Images/ciorba.jpg",
                Description = "400ml"
            });

            mockProduct.ProductDataSource.Add( new Product
            {
                Id = 32,
                Name = "Eggs",
                GroupId = 5,
                GroupName = "Breakfast",
                Image = "/Content/Images/egg.png",
                Description = "2 eggs"
            });
            mockProduct.ProductDataSource.Add( new Product
            {
                Id = 33,
                Name = "Egg bread",
                GroupId = 5,
                GroupName = "Breakfast",
                Image = "/Content/Images/paineou.jpg",
                Description = "3 slices"
            });
            mockProduct.ProductDataSource.Add( new Product
            {
                Id = 34,
                Name = "English breakfast",
                GroupId = 5,
                GroupName = "Breakfast",
                Image = "/Content/Images/micdejun.png",
                Description = "2 eggs, 1 cabanos 100g, tomato 100g, beans 80g, toast 2 slices"
            });

            mockProduct.ProductDataSource.Add( new Product
            {
                Id = 35,
                Name = "Spaghetti Carbonara",
                GroupId = 6,
                GroupName = "Spaghetti",
                Image = "/Content/Images/scarbonara.jpg",
                Description = "Pasta with ham, parmesan and sour cream"
            });
            mockProduct.ProductDataSource.Add( new Product
            {
                Id = 36,
                Checked = false,
                Name = "Bolognesse",
                GroupId = 6,
                GroupName = "Spaghetti",
                Image = "/Content/Images/sbolognesse.png",
                Description = "Pasta with bolognese sauce from beef and pork"
            });

            mockProduct.FilterGroups = mockProduct.ProductDataSource
                                                  .Select(i => new{ i.Checked, i.GroupId, i.GroupName })
                                                  .Distinct()
                                                  .ToList()
                                                  .Select(i => new FilterGroup { Checked = i.Checked, Id = i.GroupId, Name = i.GroupName })
                                                  .ToList();

            return mockProduct;
        }

        public static IEnumerable<Product> SortByName(IEnumerable<Product> dataSource, string sortType)
        {
            IEnumerable<Product> list;

            if(sortType == "desc")
            {
                list = dataSource.OrderByDescending(i => i.Name);
            }
            else
            {
                list = dataSource.OrderBy(i => i.Name);
            }

            return list;
        }

        public static IEnumerable<Product> SortByDescription(IEnumerable<Product> dataSource, string sortType)
        {
            IEnumerable<Product> list;

            if (sortType == "desc")
            {
                list = dataSource.OrderByDescending(i => i.Description);
            }
            else
            {
                list = dataSource.OrderBy(i => i.Description);
            }

            return list;
        }

        public static IEnumerable<Product> FilterByGroup(IEnumerable<Product> dataSource, List<FilterGroup> groups)
        {
            IEnumerable<Product> list;

            if (groups != null)
            {
                var groupIds = groups.Select(i => i.Id).ToList();
                list = dataSource.Where(i => groupIds.Contains(i.GroupId));
            }
            else
            {
                list = dataSource;
            }

            return list;
        }

        public static IEnumerable<Product> FilterByGroup(IEnumerable<Product> dataSource, FilterGroup group)
        {
            IEnumerable<Product> list;

            if (group != null && group.Id > -1)
            {
                var groupIds = group.Id;                
                list = dataSource.Where(i => groupIds == i.GroupId);
            }
            else
            {
                list = dataSource;
            }

            return list;
        }
    }
}