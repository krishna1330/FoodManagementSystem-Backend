using FMS.Business.Client.Models;
using FMS.Business.DatabaseObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace FMS.Data
{
    public class FoodDAC
    {
        private readonly FMS_DbContext _dbcontext;

        public FoodDAC(FMS_DbContext context)
        {
            _dbcontext = context;
        }

        public async Task<List<FoodAvailabilityData>> GetFoodAvailabilityDataAsync(int month)
        {
            try
            {

                var food = await (from fa in _dbcontext.FoodAvailability
                                  where fa.MonthNumber == month
                                  select new FoodAvailabilityData
                                  {
                                      FoodAvailabilityID = fa.FoodAvailabilityID,
                                      CreatedAdminID = fa.CreatedAdminID,
                                      MonthNumber = fa.MonthNumber,
                                      FromDate = fa.FromDate,
                                      ToDate = fa.ToDate,
                                      Menu = new List<string>()
                                  }).ToListAsync().ConfigureAwait(false);

                foreach (var f in food)
                {
                    var menuIDs = await (from fo in _dbcontext.FoodOptions
                                         where fo.FoodAvailabilityID == f.FoodAvailabilityID
                                         select fo.MenuID).ToListAsync().ConfigureAwait(false);

                    var menus = await (from m in _dbcontext.Menu
                                       where menuIDs.Contains(m.MenuID)
                                       select m.Food).ToListAsync().ConfigureAwait(false);

                    f.Menu.AddRange(menus);
                }


                return food;
            }
            catch (Exception ex)
            {
                throw new Exception("Error: ", ex);
            }
        }

        public async Task<List<MenuList>> GetMenuAsync()
        {
            try
            {
                var menu = await _dbcontext.Menu.Select(m => new MenuList
                {
                    MenuID = m.MenuID,
                    Food = m.Food,
                }).ToListAsync();

                return menu;

            }
            catch (Exception ex)
            {
                throw new Exception("Error: ", ex);
            }
        }

        public async Task<FoodAvailabilityData> AddFoodAvailabilityAsync(AddFoodAvailability addFoodAvailability)
        {
            if (addFoodAvailability.menuIDs == null || !addFoodAvailability.menuIDs.Any())
            {
                throw new ArgumentException("Menu IDs must be provided.");
            }

            try
            {
                var foodAvailability = new FoodAvailability
                {
                    MonthNumber = addFoodAvailability.FromDate.Month,
                    CreatedAdminID = addFoodAvailability.CreatedAdminID,
                    FromDate = addFoodAvailability.FromDate,
                    ToDate = addFoodAvailability.ToDate,
                    IsActive = true,
                    IsDeleted = false,
                };

                _dbcontext.FoodAvailability.Add(foodAvailability);
                await _dbcontext.SaveChangesAsync();

                foreach (int i in addFoodAvailability.menuIDs)
                {
                    var options = new FoodOptions
                    {
                        FoodAvailabilityID = foodAvailability.FoodAvailabilityID,
                        MenuID = i,
                        IsActive = true,
                        IsDeleted = false,
                    };

                    _dbcontext.FoodOptions.AddRange(options);

                    await _dbcontext.SaveChangesAsync();
                }


                var menuList = await _dbcontext.Menu
                    .Where(m => addFoodAvailability.menuIDs.Contains(m.MenuID))
                    .Select(m => m.Food)
                    .ToListAsync();

                var addedFoodAvailabilityData = new FoodAvailabilityData
                {
                    FoodAvailabilityID = foodAvailability.FoodAvailabilityID,
                    CreatedAdminID = foodAvailability.CreatedAdminID,
                    MonthNumber = foodAvailability.MonthNumber,
                    FromDate = foodAvailability.FromDate,
                    ToDate = foodAvailability.ToDate,
                    Menu = menuList
                };

                return addedFoodAvailabilityData;
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding food availability", ex);
            }
        }

        public async Task<List<UserSelectedFood>> GetUserSelectedFoodByUserIDAsync(int userID, int month)
        {
            try
            {
                var food = await (from uf in _dbcontext.UserFood
                                  where uf.SelectedDate.Month == month && uf.UserID == userID
                                  select new UserSelectedFood
                                  {
                                      UserID = userID,
                                      UserFoodID = uf.UserFoodID,
                                      MenuID = uf.MenuID,
                                      SelectedFood = _dbcontext.Menu
                                                    .Where(m => m.MenuID == uf.MenuID)
                                                    .Select(m => m.Food)
                                                    .FirstOrDefault(),
                                      SelectedDate = uf.SelectedDate,
                                      IsActive = uf.IsActive
                                  }).ToListAsync();

                return food;
            }
            catch (Exception ex)
            {
                throw new Exception("Error: ", ex);
            }
        }


        public async Task<UserSelectedFood> AddUserFoodAsync(AddUserFood addUserFood)
        {
            try
            {
                int menuID = await _dbcontext.Menu
                                             .Where(m => m.Food == addUserFood.SelectedFood)
                                             .Select(m => m.MenuID)
                                             .FirstOrDefaultAsync();

                if (menuID == 0)
                {
                    throw new Exception("Selected food is not available in the menu.");
                }

                var userFood = new UserFood
                {
                    UserID = addUserFood.UserID,
                    MenuID = menuID,
                    SelectedDate = addUserFood.SelectedDate,
                    CreatedDate = DateTime.Now,
                    IsActive = true,
                    IsDeleted = false,
                };

                _dbcontext.UserFood.Add(userFood);
                await _dbcontext.SaveChangesAsync();

                var food = await (from uf in _dbcontext.UserFood
                                  join m in _dbcontext.Menu on uf.MenuID equals m.MenuID
                                  where uf.UserFoodID == userFood.UserFoodID
                                  select new UserSelectedFood
                                  {
                                      UserID = uf.UserID,
                                      UserFoodID = uf.UserFoodID,
                                      MenuID = uf.MenuID,
                                      SelectedFood = m.Food, // Now directly getting the food from the joined Menu table
                                      SelectedDate = uf.SelectedDate,
                                      IsActive = uf.IsActive
                                  })
                                  .FirstOrDefaultAsync();


                return food;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding the selected food.", ex);
            }
        }

        public async Task<FoodCount> GetEmployeeFoodCountAsync(DateTime selectedDate)
        {
            try
            {
                var foodAvailabilityID = await _dbcontext.FoodAvailability
                    .Where(f => (f.FromDate <= selectedDate && f.ToDate >= selectedDate))
                    .Select(f => f.FoodAvailabilityID)
                    .FirstOrDefaultAsync();

                if (foodAvailabilityID == 0)
                {
                    throw new Exception("No food availability found for the selected date.");
                }

                var menuIDs = await _dbcontext.FoodOptions
                    .Where(m => m.FoodAvailabilityID == foodAvailabilityID)
                    .Select(m => m.MenuID)
                    .ToListAsync();

                var foodCount = new FoodCount
                {
                    SelectedDate = selectedDate,
                };

                var foodCounts = await _dbcontext.UserFood
                    .Where(uf => uf.SelectedDate == selectedDate && menuIDs.Contains(uf.MenuID))
                    .GroupBy(uf => uf.MenuID)
                    .Select(group => new
                    {
                        MenuID = group.Key,
                        Count = group.Count()
                    })
                    .ToListAsync();

                foreach (var item in foodCounts)
                {
                    var food = await _dbcontext.Menu
                        .Where(m => m.MenuID == item.MenuID)
                        .Select(m => m.Food)
                        .FirstOrDefaultAsync();

                    if (food != null)
                    {
                        foodCount.Food_Count.Add(food, item.Count);
                    }
                }

                return foodCount;
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching employee food count: ", ex);
            }
        }
    }
}
