using System.Collections.Generic;
using CommitteeManagement.Model;
using CommitteeManagement.Repository.Data;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace CommitteeManagement.Repository.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<CommitteeManagement.Repository.Data.DataContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            //DropCreateDatabaseIfModelChanges<>
        }

        protected override void Seed(CommitteeManagement.Repository.Data.DataContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //


            CreateUser(context, "SuperAdmin", "SuperAdminCommittee", "vahid.asbaghi@gmail.com", "Aa@123456");

            #region Add criterion/subcriterion/indicator/idealvalue/real value mock data

            var committee = context.Committees.FirstOrDefault(committeeT => committeeT.Name == "SuperAdminCommittee");
            var user = context.Users.FirstOrDefault(userT => userT.UserName == "vahid.asbaghi@gmail.com");
            var criteria = CriterionMockDataGenerate(5, committee, context, user);
            var subCriterions = SubCriterionMockDataGenerate(criteria, new List<int>() { 3, 4, 3, 2, 2 }, context, user);
            var numberIndicators = new List<int>();
            Random random = new Random(DateTime.Now.GetHashCode());
            for (int i = 0; i < subCriterions.Count; i++)
            {
                numberIndicators.Add(random.Next(5));
            }
            var indicators = IndicatorsMockDataGenerate(subCriterions, numberIndicators, context, user);

            foreach (var indicator in indicators)
            {
                double days = -500;
                double? value = null;
                bool? moreThan = null;
                for (int i = 0; i < 10; i++)
                {
                    var minDateTime = DateTime.Now.AddDays(days);
                    var idealValue = IndicatorIdealValueMock(indicator, context, minDateTime, moreThan, value, user);
                    for (int j = 0; j < 10; j++)
                    {
                        var realValue = IndicatorRealValueMock(idealValue, context, minDateTime, user);
                        minDateTime = realValue.Time.GetValueOrDefault();
                        if (random.Next(0, 10) > 6)
                        {
                            break;
                        }
                    }
                    days += 50;
                    moreThan = idealValue.MoreThan;
                    value = idealValue.Value + random.Next((int)(-idealValue.Value / 1000), (int)(idealValue.Value / 1000));
                    if (random.Next(0, 10) > 6)
                        break;
                }

            }

            #endregion
        }

        private void CreateUser(DataContext context, string roleName, string committeeName, string userName, string password)
        {
            var role = new Role() { Name = roleName };
            context.Roles.Add(role); //add role to db to get id
            context.SaveChanges();
            var committee = new Committee() { Name = committeeName };
            context.Committees.Add(committee); //add committee to get id
            context.SaveChanges();
            var getCommittee = context.Committees.FirstOrDefault(c => c.Name == committee.Name);
            if (getCommittee == null) return;

            var user = new User { UserName = userName, Email = userName, CommitteeRefId = getCommittee.Id, Committee = getCommittee };
            using (var userManager = new UserManager<User>(new UserStore<User>(context)))
            {
                var result = userManager.Create<User, string>(user, password); //add user using userManager to hash password
                if (!result.Succeeded)
                {
                    return;
                }
            }

            var getUser = context.Users.FirstOrDefault(userT => userT.UserName == userName);//get added user
            if (getUser == null) return;
            var identityRole = context.Roles.FirstOrDefault(roleT => roleT.Name == role.Name);//get added role
            if (identityRole != null)
            {
                var newRole = new IdentityUserRole() { RoleId = identityRole.Id, UserId = getUser.Id }; //create identityUserRole to add to roles and users tables
                user.Roles.Add(newRole);
                role.Users.Add(newRole);
            }
            context.Users.Attach(user);
            context.Roles.Attach(role);
            context.SaveChanges();
        }

        /// <summary>
        /// Criteria the mock data generate.
        /// </summary>
        /// <param name="numberOfCriterion">The number of criterion.</param>
        /// <param name="committee">The committee.</param>
        /// <param name="dbContext"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public List<Criterion> CriterionMockDataGenerate(int numberOfCriterion, Committee committee, DataContext dbContext, User user)
        {
            var criterionSubjects = new List<string>
            {
                "توسعه ظرفيت انساني",
                "توسعه و استفاده از زیرساخت‌ها",
                "تعامل با بازار",
                "سطح شناخت و تعامل با فضای کسب‌وکار",
                "خروجی‌های علمي",
                "خروجی‌های خلاقانه و رفاه اجتماعي",
                "تدوين و پیاده‌سازی سیاست‌ها در سطح معاونت"
            };
            var criteria = new List<Criterion>();
            var randomCoefficient = new Random();
            var maxRandom = 100;
            var sumRandom = 0;
            for (int i = 0; i < numberOfCriterion; i++)
            {
                var tempIntCoefficient = randomCoefficient.Next(Math.Min(5, maxRandom - sumRandom - 1), maxRandom - sumRandom);
                if (tempIntCoefficient > maxRandom / 2)
                {
                    tempIntCoefficient /= 2;
                }
                var criterion = new Criterion
                {
                    Committee = committee,
                    Coefficient = (double)tempIntCoefficient / maxRandom,
                    CommitteeId = committee.Id,
                    IsDeleted = false,
                    Subject = criterionSubjects[randomCoefficient.Next(0, criterionSubjects.Count - 1)]
                };
                if (i == numberOfCriterion - 1)
                {
                    criterion.Coefficient = (maxRandom - sumRandom) / maxRandom;
                }
                sumRandom += tempIntCoefficient;
                criteria.Add(criterion);
                var addedCriterion = dbContext.Criteria.Add(criterion);
                dbContext.SaveChanges();
                var criterionModification = new CriterionModification()
                {
                    Add = true,
                    Criterion = addedCriterion,
                    Delete = false,
                    Time = DateTime.Now,
                    Update = false,
                    User = user
                };
                var addedCriterionModification = dbContext.CriterionModifications.Add(criterionModification);
                dbContext.SaveChanges();

                addedCriterion.CriterionModifications.Add(addedCriterionModification);
                dbContext.Criteria.Attach(addedCriterion);

            }
            dbContext.SaveChanges();
            return criteria;
        }

        /// <summary>
        /// SubCriterions mock data generator.
        /// </summary>
        /// <param name="criteria"></param>
        /// <param name="numberOfSubCriterion">The number of sub criterion.</param>
        /// <returns></returns>
        public List<SubCriterion> SubCriterionMockDataGenerate(List<Criterion> criteria, List<int> numberOfSubCriterion, DataContext dbContext, User user)
        {
            var subCriterionSubjects = new List<string>
            {
                "سرمایه‌گذاری در آموزش و توانمندسازي نيروي كار (آموزش عالي و آموزش عمومی و حرفه‌ای)",
                "توسعه کمی و کیفی مشاغل",
                "زيرساخت عمومي (استفاده حداكثري از توان داخلي و فعال‌سازی ظرفیت‌ها)",
                "حمایت هدفمند از صنایع، خدمات و شرکت‌های دانش‌بنیان بر اساس سطح توسعه‌یافتگی و توان ارزشآفرینی",
                "حمایت مالی، معنوي و تسهیل جذب و استفاده از منابع بازار‌های مالی و سرمایه‌ای داخلی و خارجی در جهت ارتقای توان و ارزش‌آفرینی",
                "توسعه هوشمند و فعال همکاری‌ها و تعاملات بین‌المللی (به‌منظور پيشبرد برنامه‌های توسعه فناوري، دستيابي به فناوری‌های پيشرفته، انتقال فناوري و تجاری‌سازی محصولات دانش‌بنیان)",
                "جاری‌سازی",
                "فعالیت‌هایی از جنس تحقيق و توسعه",
                "تجارت خارجي و رقابت‌پذیری",
                "اكوسيستم نوآوري"
            };
            var subCriterions = new List<SubCriterion>();
            var randomCoefficient = new Random();
            var maxRandom = 100;
            var sumRandom = 0;
            int counterNum = 0;
            for (int j = 0; j < criteria.Count; j++)
            {
                var criterion = criteria[j];
                sumRandom = 0;
                for (int i = 0; i < numberOfSubCriterion[j]; i++)
                {
                    var tempIntCoefficient = randomCoefficient.Next(Math.Min(5, maxRandom - sumRandom - 1),
                        maxRandom - sumRandom);
                    if (tempIntCoefficient > maxRandom / 2)
                    {
                        tempIntCoefficient /= 2;
                    }
                    var subCriterion = new SubCriterion()
                    {
                        Coefficient = (double)tempIntCoefficient / maxRandom,
                        Criterion = criterion,
                        CriterionId = criterion.Id,
                        Subject = subCriterionSubjects[randomCoefficient.Next(0, subCriterionSubjects.Count - 1)],
                        Committee = criterion.Committee
                    };
                    if (i == numberOfSubCriterion[j] - 1)
                    {
                        subCriterion.Coefficient = (double)(maxRandom - sumRandom) / maxRandom;
                    }
                    sumRandom += tempIntCoefficient;
                    subCriterions.Add(subCriterion);
                    counterNum++;
                    var addedSubCriterion = dbContext.SubCriterions.Add(subCriterion);
                    dbContext.SaveChanges();
                    var subCriterionModification = new SubCriterionModification()
                    {
                        Add = true,
                        SubCriterion = addedSubCriterion,
                        Time = DateTime.Now,
                        User = user
                    };
                    var addedSubCriterionModification = dbContext.SubCriterionModifications.Add(subCriterionModification);
                    dbContext.SaveChanges();

                    addedSubCriterion.SubCriterionModifications.Add(addedSubCriterionModification);
                    dbContext.SubCriterions.Attach(addedSubCriterion);
                }
            }
            dbContext.SaveChanges();

            return subCriterions;
        }


        public List<Indicator> IndicatorsMockDataGenerate(List<SubCriterion> subCriterions, List<int> numberOfIndicators, DataContext dataContext, User user)
        {
            var indicatorSubjects = new List<string>
            {
                "ميزان هزينه كرد بودجه براي توسعه سرمايه انساني ستادهاي معاونت",
                "تعداد دانشجويان و فارغ‌التحصیل مؤسسات آموزش عالي كشور در حوزه‌های تخصصي ستادهاي معاونت",
                "تعداد دانشگاه‌ها و مؤسسات پژوهشي فعال كشور در حوزه‌های تخصصي ستادهاي معاونت",
                "تعداد پژوهشگران معاونت از کل پژوهشگران كشور",
                "تعداد اعضاي هیئت‌علمی ستادهاي معاونت",
                "تعداد شاغلین (متخصصین) معاونت از کل شاغلین کشور",
                "	متوسط هزینه کرد برای ایجاد هر شغل در معاونت (ريال)",
                "ميزان ساعت خدمات مشاوره‌ای شغلي ارائه‌شده جهت بهبود عملكرد كاركنان كل معاونت در سال",
                "ميزان حمایت مادي از راه‌اندازی و توسعه مراکز نوآوري (انستيتو تحقيقاتي ملي و مركز بین‌المللی تحقيق و توسعه) و شهرک‌های صنعتی تخصصی و صنایع کوچك و متوسط، توسط معاونت",
                "ميزان بودجه اختصاص‌یافته براي تکمیل، تجهیز و توانمندسازی آزمایشگاه¬های موجود و احداث آزمایشگاه‌های تخصصی ملی موردنیاز در بخش توسعه فناوری، دانش فنی و تشکیل شبکه ملی آزمایشگاهی در ستادهاي معاونت",
                "تعداد (مجموع) پارک‌ها و مراکز رشد فعال ستادهاي معاونت"
            };
            var indicators = new List<Indicator>();
            var randomCoefficient = new Random();
            var maxRandom = 100;
            var sumRandom = 0;
            var counterNum = 0;
            for (int j = 0; j < subCriterions.Count; j++)
            {
                var subCriterion = subCriterions[j];
                sumRandom = 0;
                for (int i = 0; i < numberOfIndicators[j]; i++)
                {
                    var tempIntCoefficient = randomCoefficient.Next(Math.Min(5, maxRandom - sumRandom - 1),
                        maxRandom - sumRandom);
                    if (tempIntCoefficient > maxRandom / 2)
                    {
                        tempIntCoefficient /= 2;
                    }
                    var indicator = new Indicator
                    {
                        Coefficient = (double)tempIntCoefficient / maxRandom,
                        DeadlinePeriod = randomCoefficient.Next(30, 90),
                        SubCriterion = subCriterion,
                        Subject = indicatorSubjects[randomCoefficient.Next(0, indicatorSubjects.Count - 1)],
                        Committee = subCriterion.Committee
                    };
                    if (i == numberOfIndicators[j] - 1)
                    {
                        indicator.Coefficient = (double)(maxRandom - sumRandom) / maxRandom;
                    }
                    sumRandom += tempIntCoefficient;
                    indicators.Add(indicator);
                    counterNum++;
                    var addedIndicator = dataContext.Indicators.Add(indicator);
                    dataContext.SaveChanges();
                    var indicatorModification = new IndicatorModification
                    {
                        AddIndicator = true,
                        Indicator = addedIndicator,
                        Time = DateTime.Now,
                        User = user,
                    };
                    var addedIndicatorModification = dataContext.IndicatorModifications.Add(indicatorModification);
                    dataContext.SaveChanges();

                    addedIndicator.IndicatorModifications.Add(addedIndicatorModification);
                    dataContext.Indicators.Attach(addedIndicator);
                }
            }
            dataContext.SaveChanges();
            return indicators;
        }

        /// <summary>
        /// Indicators the ideal value mock.
        /// </summary>
        /// <param name="indicator">The indicator.</param>
        /// <param name="dataContext">The data context.</param>
        /// <param name="time"></param>
        /// <param name="moreThan1"></param>
        /// <param name="value1"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public IndicatorIdealValue IndicatorIdealValueMock(Indicator indicator, DataContext dataContext, DateTime time, bool? moreThan1, double? value1, User user)
        {
            //Tuples: moreThan=true(bool), value=double
            var values = new List<Tuple<double, bool>>()
            {
                new Tuple<double, bool>(2.5,true),
                new Tuple<double, bool>(50000,true),
                new Tuple<double, bool>(800,true),
                new Tuple<double, bool>(5000,true),
                new Tuple<double, bool>(20000,true),
                new Tuple<double, bool>(100000,true),
                new Tuple<double, bool>(1000000000,false),
                new Tuple<double, bool>(5000,true),
            };

            Random random = new Random(DateTime.Now.GetHashCode());
            var value = value1 ?? values[random.Next(values.Count)].Item1;
            var moreThan = moreThan1 ?? values[random.Next(values.Count)].Item2;
            var idealValue = new IndicatorIdealValue()
            {
                Indicator = indicator,
                Value = value,
                LowerThan = !moreThan,
                MoreThan = moreThan,
                Time = time
            };
            dataContext.IndicatorIdealValues.Add(idealValue);
            var indicatorModification = new IndicatorModification()
            {
                AddIdealValue = true,
                Indicator = indicator,
                Time = time,
                User = user
            };
            var addedIndicatorModification = dataContext.IndicatorModifications.Add(indicatorModification);
            dataContext.SaveChanges();

            indicator.IndicatorModifications.Add(addedIndicatorModification);
            dataContext.Indicators.Attach(indicator);

            dataContext.SaveChanges();
            return idealValue;
        }

        /// <summary>
        /// Indicators the real value mock.
        /// </summary>
        /// <param name="idealValue">The ideal value.</param>
        /// <param name="dataContext">The data context.</param>
        /// <param name="minDateTime"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public IndicatorRealValue IndicatorRealValueMock(IndicatorIdealValue idealValue, DataContext dataContext, DateTime? minDateTime, User user)
        {
            Random random = new Random(DateTime.Now.GetHashCode());
            var realValue = new IndicatorRealValue()
            {
                Indicator = idealValue.Indicator,
                Time = GenerateRandomDateTime(minDateTime),
                Value =
                    random.Next((int)(idealValue.Value - idealValue.Value / 100),
                        (int)(idealValue.Value + idealValue.Value / 100))
            };
            var addedRealValue = dataContext.IndicatorRealValues.Add(realValue);
            dataContext.SaveChanges();

            var indicatorModification = new IndicatorModification()
            {
                AddRealValue = true,
                Indicator = idealValue.Indicator,
                Time = realValue.Time,
                User = user
            };
            var addedIndicatorModification = dataContext.IndicatorModifications.Add(indicatorModification);
            dataContext.SaveChanges();

            var indicator = idealValue.Indicator;
            indicator.IndicatorModifications.Add(addedIndicatorModification);
            dataContext.Indicators.Attach(indicator);

            realValue.Indicator = indicator;
            dataContext.IndicatorRealValues.Attach(realValue);

            dataContext.SaveChanges();
            return realValue;
        }

        private DateTime GenerateRandomDateTime(DateTime? minDateTime)
        {
            var random = new Random(DateTime.Now.GetHashCode());

            return minDateTime?.AddHours(random.Next(0, 24 * 49)) ?? DateTime.MinValue.AddDays(random.Next((DateTime.Now - DateTime.MinValue).Days));
        }
    }

}
