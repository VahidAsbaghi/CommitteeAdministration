using System.Collections.Generic;
using CommitteeManagement.Model;
using CommitteeManagement.Repository.Data;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;

namespace CommitteeManagement.Repository.Migrations
{


    internal sealed class Configuration : MigrateDatabaseToLatestVersion<DataContext, Configuration.AppDbMigrationConfiguration>
    {
        public class AppDbMigrationConfiguration : DbMigrationsConfiguration<DataContext>
        {
            public AppDbMigrationConfiguration()
            {
                // If you want automatic migrations the uncomment the line below.
                //AutomaticMigrationsEnabled = true;
                AutomaticMigrationsEnabled = true;
                AutomaticMigrationDataLossAllowed = true;
            }

            protected override void Seed(DataContext context)
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
                var randomCoefficient = new Random();
                var user = context.Users.FirstOrDefault(userT => userT.UserName == "vahid.asbaghi@gmail.com");
                if (user == null)
                {
                    CreateUser(context, "SuperAdmin", "SuperAdminCommittee", "vahid.asbaghi@gmail.com", "Aa@123456");
                }
               

                //#region Add criterion/subcriterion/indicator/idealvalue/real value mock data

                var committee = context.Committees.FirstOrDefault(committeeT => committeeT.Name == "SuperAdminCommittee");
               

                #region معیارها

                var criterions = new List<Criterion>();
                var criterion1 = new Criterion()
                {
                    Committee = committee,
                    Subject = " توانمندسازی سرمایه انساني و ترویج",
                    Coefficient = 0.122136831,
                };
                criterions.Add(criterion1);
                var criterion2 = new Criterion()
                {
                    Committee = committee,
                    Subject = "منابع مالی و زیرساخت",
                    Coefficient = 0.145316323

                };
                criterions.Add(criterion2);

                var criterion3 = new Criterion()
                {
                    Committee = committee,
                    Subject = "تعاملات بین المللی",
                    Coefficient = 0.146035527
                    ,
                };
                criterions.Add(criterion3);
                var criterion4 = new Criterion()
                {
                    Committee = committee,
                    Subject = " دستاوردهای اقتصادی",
                    Coefficient = 0.159516379
                };
                criterions.Add(criterion4);

                var criterion5 = new Criterion()
                {
                    Committee = committee,
                    Subject = "تجاری‏سازی و توسعه بازار",
                    Coefficient = 0.148033316
                };
                criterions.Add(criterion5);
                var criterion6 = new Criterion()
                {
                    Committee = committee,
                    Subject = "تولیدات علمی و فناورانه",
                    Coefficient = 0.136513673
                };
                criterions.Add(criterion6);
                var criterion7 = new Criterion()
                {
                    Committee = committee,
                    Subject = "فعالیت‏های اجرایی ستاد",
                    Coefficient = 0.142447949

                };
                criterions.Add(criterion7);

                criterions.ForEach(n => context.Criteria.AddOrUpdate(n));
                context.SaveChanges();

                #endregion


                #region زیر معیارها

                var subcriterions = new List<SubCriterion>();

                var criterion_1_1_Sub = new SubCriterion()
                {
                    Criterion = criterion1,
                    Coefficient = 0.530025078,
                    Subject = "آموزش و توانمندسازي نیروی انسانی",

                };
                subcriterions.Add(criterion_1_1_Sub);

                var criterion_1_2_Sub = new SubCriterion()
                {
                    Criterion = criterion1,
                    Coefficient = 0.469974922,
                    Subject = "ترویج و فرهنگ‏سازی",

                };
                subcriterions.Add(criterion_1_2_Sub);



                var criterion_2_1_Sub = new SubCriterion()
                {
                    Criterion = criterion2,
                    Coefficient = 0.475335177,
                    Subject = "منابع مالی تحقيق و توسعه و فعالیت‌های آموزشي و پژوهشي",

                };
                subcriterions.Add(criterion_2_1_Sub);

                var criterion_2_2_Sub = new SubCriterion()
                {
                    Criterion = criterion2,
                    Coefficient = 0.524664823,
                    Subject = "زيرساخت توسعه پژوهش، فناوری و اقتصاد دانش بنیان",

                };
                subcriterions.Add(criterion_2_2_Sub);



                var criterion_3_1_Sub = new SubCriterion()
                {
                    Criterion = criterion3,
                    Coefficient = 1,
                    Subject = "توسعه همکاری‏ها و تعاملات بین‌المللی",

                };
                subcriterions.Add(criterion_3_1_Sub);



                var criterion_4_1_Sub = new SubCriterion()
                {
                    Criterion = criterion4,
                    Coefficient = 0.531671694,
                    Subject = "زيرساخت توسعه پژوهش، فناوری و اقتصاد دانش بنیان",

                };
                subcriterions.Add(criterion_4_1_Sub);
                var criterion_4_2_Sub = new SubCriterion()
                {
                    Criterion = criterion4,
                    Coefficient = 0.468328306,
                    Subject = "زيرساخت توسعه پژوهش، فناوری و اقتصاد دانش بنیان",

                };
                subcriterions.Add(criterion_4_2_Sub);


                var criterion_5_1_Sub = new SubCriterion()
                {
                    Criterion = criterion5,
                    Coefficient = 0.339859342,
                    Subject = "انتقال فناوری",

                };
                subcriterions.Add(criterion_5_1_Sub);
                var criterion_5_2_Sub = new SubCriterion()
                {
                    Criterion = criterion5,
                    Coefficient = 0.31343851,
                    Subject = "توسعه بازار",

                };
                subcriterions.Add(criterion_5_2_Sub);
                var criterion_5_3_Sub = new SubCriterion()
                {
                    Criterion = criterion5,
                    Coefficient = 0.346702148,
                    Subject = "تجاری‌سازی",

                };
                subcriterions.Add(criterion_5_3_Sub);



                var criterion_6_1_Sub = new SubCriterion()
                {
                    Criterion = criterion6,
                    Coefficient = 0.445955973,
                    Subject = "انتشارات و دستاوردهای علمی",

                };
                subcriterions.Add(criterion_6_1_Sub);
                var criterion_6_2_Sub = new SubCriterion()
                {
                    Criterion = criterion6,
                    Coefficient = 0.554044027,
                    Subject = "دارائی‏های فکری",

                };
                subcriterions.Add(criterion_6_2_Sub);


                var criterion_7_1_Sub = new SubCriterion()
                {
                    Criterion = criterion7,
                    Coefficient = 1,
                    Subject = "فعالیت‏های اجرایی ستاد",

                };
                subcriterions.Add(criterion_7_1_Sub);


                subcriterions.ForEach(n => context.SubCriterions.AddOrUpdate(n));
                context.SaveChanges();

                #endregion



                #region شاخص ها

                var indicators = new List<Indicator>();
                var indic_1_1_1 = new Indicator()
                {
                    Coefficient = 0.110092769,
                    Subject = "تعداد دانشجويان كشور در آن حوزه‌",
                    SubCriterion = criterion_1_1_Sub,
                    DeadlinePeriod = randomCoefficient.Next(30, 90),

                };
                indicators.Add(indic_1_1_1);

                var indic_1_1_2 = new Indicator()
                {
                    Coefficient = 0.126858165,
                    Subject = "تعداد فارغ‌التحصیل كشور در آن حوزه‌",
                    SubCriterion = criterion_1_1_Sub,
                };
                indicators.Add(indic_1_1_2);

                var indic_1_1_3 = new Indicator()
                {
                    Coefficient = 0.134682016,
                    Subject = "تعداد دانشگاه‌ها و مؤسسات آموزشی فعال كشور در آن حوزه",
                    SubCriterion = criterion_1_1_Sub,
                };
                indicators.Add(indic_1_1_3);

                var indic_1_1_4 = new Indicator()
                {
                    Coefficient = 0.163742036,
                    Subject = "تعداد پژوهشگران آن حوزه و نسبت آن به کل پژوهشگران كشور",
                    SubCriterion = criterion_1_1_Sub,
                };
                indicators.Add(indic_1_1_4);

                var indic_1_1_5 = new Indicator()
                {
                    Coefficient = 0.14697664,
                    Subject = "تعداد اعضاي هیئت‌علمی در آن حوزه",
                    SubCriterion = criterion_1_1_Sub,
                };
                indicators.Add(indic_1_1_5);

                var indic_1_1_6 = new Indicator()
                {
                    Coefficient = 0.141388175,
                    Subject = "تعداد و میزان حمایت از برگزاری کارگاه‏های آموزشی",
                    SubCriterion = criterion_1_1_Sub,
                };
                indicators.Add(indic_1_1_6);

                var indic_1_1_7 = new Indicator()
                {
                    Coefficient = 0.176260199,
                    Subject =
                        "تعداد نيروي انساني تحقيق و توسعه آن حوزه و  نسبت آن به‌کل نيروي انساني تحقيق و توسعه کشور‌ ",
                    SubCriterion = criterion_1_1_Sub,
                };
                indicators.Add(indic_1_1_7);


                var indic_1_2_1 = new Indicator()
                {
                    Coefficient = 0.325,
                    Subject = "تعداد نشست‏های تخصصی، سمینارها، همایش‌های ملی و بین المللی برگزار شده در آن حوزه‌",
                    SubCriterion = criterion_1_2_Sub,
                };
                indicators.Add(indic_1_2_1);

                var indic_1_2_2 = new Indicator()
                {
                    Coefficient = 0.347058824,
                    Subject = "تعداد و میزان حمایت از جشنواره‌ها، المپیادها و مسابقات تخصصی‌",
                    SubCriterion = criterion_1_2_Sub,
                };
                indicators.Add(indic_1_2_2);

                var indic_1_2_3 = new Indicator()
                {
                    Coefficient = 0.327941176,
                    Subject = "تعداد مجلات تخصصی آن حوزه",
                    SubCriterion = criterion_1_2_Sub,
                };
                indicators.Add(indic_1_2_3);



                //معیار دوم-زیر معیار اول

                var indic_2_1_1 = new Indicator()
                {
                    Coefficient = 0.24094442,
                    Subject = "میزان هزینه‏کرد تحقیق و توسعه آن حوزه  و سهم آن از کل هزینه‏کرد تحقیق و توسعه کشور",
                    SubCriterion = criterion_1_1_Sub,
                };
                indicators.Add(indic_2_1_1);


                //معیار دوم-زیر معیار اول
                var indic_2_1_2 = new Indicator()
                {
                    Coefficient = 0.23625165,
                    Subject = "تعداد و میزان حمایت‌های صورت گرفته ستاد جهت انجام فعالیت‌های تحقيق و توسعه",
                    SubCriterion = criterion_2_1_Sub,
                };
                indicators.Add(indic_2_1_2);

                var indic_2_1_3 = new Indicator()
                {
                    Coefficient = 0.185511072,
                    Subject = "تعداد و میزان منابع تخصیص یافته ستاد به منظور حمایت از طرح‏های پژوهشی (طرفا پژوهشی)",
                    SubCriterion = criterion_2_1_Sub,
                };
                indicators.Add(indic_2_1_3);

                var indic_2_1_4 = new Indicator()
                {
                    Coefficient = 0.183311336,
                    Subject = "تعداد و میزان حمایت از پژوهشسراها در بخش تحقیق و پژوهش",
                    SubCriterion = criterion_2_1_Sub,
                };
                indicators.Add(indic_2_1_4);

                var indic_2_1_5 = new Indicator()
                {
                    Coefficient = 0.153981522,
                    Subject = "تعداد و میزان اعطای پژوهانه‏های تحقیقاتی به دانشجویان و پژوهشگران",
                    SubCriterion = criterion_2_1_Sub,
                };
                indicators.Add(indic_2_1_5);



                //معیار دوم-زیر معیار دوم

                var indic_2_2_1 = new Indicator()
                {
                    Coefficient = 0.23625165,
                    Subject = "تعداد و میزان حمایت‌های صورت گرفته ستاد جهت انجام فعالیت‌های تحقيق و توسعه",
                    SubCriterion = criterion_2_2_Sub,
                };
                indicators.Add(indic_2_2_1);

                var indic_2_2_2 = new Indicator()
                {
                    Coefficient = 0.185511072,
                    Subject = "تعداد و میزان منابع تخصیص یافته ستاد به منظور حمایت از طرح‏های پژوهشی (طرفا پژوهشی)",
                    SubCriterion = criterion_2_2_Sub,
                };
                indicators.Add(indic_2_2_2);

                var indic_2_2_3 = new Indicator()
                {
                    Coefficient = 0.183311336,
                    Subject = "تعداد و میزان حمایت از پژوهشسراها در بخش تحقیق و پژوهش",
                    SubCriterion = criterion_2_2_Sub,
                };
                indicators.Add(indic_2_2_3);

                var indic_2_2_4 = new Indicator()
                {
                    Coefficient = 0.153981522,
                    Subject = "تعداد و میزان اعطای پژوهانه‏های تحقیقاتی به دانشجویان و پژوهشگران",
                    SubCriterion = criterion_2_2_Sub,
                };
                indicators.Add(indic_2_2_4);

                var indic_2_2_5 = new Indicator()
                {
                    Coefficient = 0.24094442,
                    Subject = "میزان هزینه‏کرد تحقیق و توسعه آن حوزه  و سهم آن از کل هزینه‏کرد تحقیق و توسعه کشور",
                    SubCriterion = criterion_2_2_Sub,
                };
                indicators.Add(indic_2_2_5);

                var indic_2_2_6 = new Indicator()
                {
                    Coefficient = 0.23625165,
                    Subject = "تعداد و میزان حمایت‌های صورت گرفته ستاد جهت انجام فعالیت‌های تحقيق و توسعه",
                    SubCriterion = criterion_2_2_Sub,
                };
                indicators.Add(indic_2_2_6);

                var indic_2_2_7 = new Indicator()
                {
                    Coefficient = 0.185511072,
                    Subject = "تعداد و میزان منابع تخصیص یافته ستاد به منظور حمایت از طرح‏های پژوهشی (طرفا پژوهشی)",
                    SubCriterion = criterion_2_2_Sub,
                };
                indicators.Add(indic_2_2_7);

                var indic_2_2_8 = new Indicator()
                {
                    Coefficient = 0.183311336,
                    Subject = "تعداد و میزان حمایت از پژوهشسراها در بخش تحقیق و پژوهش",
                    SubCriterion = criterion_2_2_Sub,
                };
                indicators.Add(indic_2_2_8);

                var indic_2_2_9 = new Indicator()
                {
                    Coefficient = 0.153981522,
                    Subject = "تعداد و میزان اعطای پژوهانه‏های تحقیقاتی به دانشجویان و پژوهشگران",
                    SubCriterion = criterion_2_2_Sub,
                };
                indicators.Add(indic_2_2_9);

                var indic_2_2_10 = new Indicator()
                {
                    Coefficient = 0.24094442,
                    Subject = "میزان هزینه‏کرد تحقیق و توسعه آن حوزه  و سهم آن از کل هزینه‏کرد تحقیق و توسعه کشور",
                    SubCriterion = criterion_2_2_Sub,
                };
                indicators.Add(indic_2_2_10);

                var indic_2_2_11 = new Indicator()
                {
                    Coefficient = 0.153981522,
                    Subject = "تعداد و میزان اعطای پژوهانه‏های تحقیقاتی به دانشجویان و پژوهشگران",
                    SubCriterion = criterion_2_2_Sub,
                };
                indicators.Add(indic_2_2_11);

                var indic_2_2_12 = new Indicator()
                {
                    Coefficient = 0.24094442,
                    Subject = "میزان هزینه‏کرد تحقیق و توسعه آن حوزه  و سهم آن از کل هزینه‏کرد تحقیق و توسعه کشور",
                    SubCriterion = criterion_2_2_Sub,
                };
                indicators.Add(indic_2_2_12);



                //معیار سوم - زیر معیار اول
                var indic_3_1_1 = new Indicator()
                {
                    Coefficient = 0.123699422,
                    Subject = "تعداد و میزان پروژه‌های مشترک پژوهش و فناوری با دیگر کشورها",
                    SubCriterion = criterion_3_1_Sub,
                };
                indicators.Add(indic_3_1_1);

                var indic_3_1_2 = new Indicator()
                {
                    Coefficient = 0.105587669,
                    Subject = "تعداد رویدادهای مشترک (همایش و نمایشگاه) با دیگر کشورها",
                    SubCriterion = criterion_3_1_Sub,
                };
                indicators.Add(indic_3_1_2);

                var indic_3_1_3 = new Indicator()
                {
                    Coefficient = 0.105587669,
                    Subject = "تعداد اساتید خارجی کشور در آن حوزه",
                    SubCriterion = criterion_3_1_Sub,
                };
                indicators.Add(indic_3_1_3);

                var indic_3_1_4 = new Indicator()
                {
                    Coefficient = 0.104046243,
                    Subject = "تعداد متخصصین خارجی شاغل در صنعت آن حوزه",
                    SubCriterion = criterion_3_1_Sub,
                };
                indicators.Add(indic_3_1_4);

                var indic_3_1_5 = new Indicator()
                {
                    Coefficient = 0.100963391,
                    Subject = "تعداد دانشجویان خارجی در کشور ",
                    SubCriterion = criterion_3_1_Sub,
                };
                indicators.Add(indic_3_1_5);

                var indic_3_1_6 = new Indicator()
                {
                    Coefficient = 0.105973025,
                    Subject = "تعداد دانشجویان ایرانی در دیگر کشورها",
                    SubCriterion = criterion_3_1_Sub,
                };
                indicators.Add(indic_3_1_6);

                var indic_3_1_7 = new Indicator()
                {
                    Coefficient = 0.101734104,
                    Subject = "تعداد مقالات مشترک با دیگر کشورهای در آن حوزه  و سهم آن از کل مقالات در آن حوزه",
                    SubCriterion = criterion_3_1_Sub,
                };
                indicators.Add(indic_3_1_7);

                var indic_3_1_8 = new Indicator()
                {
                    Coefficient = 0.122157996,
                    Subject = "تعداد و میزان تفاهم‌نامه و قراردادهای بین‌المللی منعقد شده با دیگر کشورها ",
                    SubCriterion = criterion_3_1_Sub,
                };
                indicators.Add(indic_3_1_8);

                var indic_3_1_9 = new Indicator()
                {
                    Coefficient = 0.130250482,
                    Subject = "میزان سرمایه‏گذاری خارجی در آن حوزه",
                    SubCriterion = criterion_3_1_Sub,
                };
                indicators.Add(indic_3_1_9);


                ////معیارچهارم-زیر معیار اول

                var indic_4_1_1 = new Indicator()
                {
                    Coefficient = 0.253807107,
                    Subject = "حجم صادرات آن حوزه و سهم آن از كل صادرات غیرنفتی كشور",
                    SubCriterion = criterion_4_1_Sub,
                };
                indicators.Add(indic_4_1_1);


                var indic_4_1_2 = new Indicator()
                {
                    Coefficient = 0.255747984,
                    Subject = " سهم آن حوزه از تولید ناخالص داخلی",
                    SubCriterion = criterion_4_1_Sub,
                };
                indicators.Add(indic_4_1_2);

                var indic_4_1_3 = new Indicator()
                {
                    Coefficient = 0.253807107,
                    Subject = " سهم آن حوزه از رشد ساليانه توليد ناخالص داخلي",
                    SubCriterion = criterion_4_1_Sub,
                };
                indicators.Add(indic_4_1_3);

                var indic_4_1_4 = new Indicator()
                {
                    Coefficient = 0.236637802,
                    Subject = "میزان حمایت‏های توانمندسازی در توسعه صادرات",
                    SubCriterion = criterion_4_1_Sub,
                };
                indicators.Add(indic_4_1_4);

                //معیار چهارم- زیر معیار دوم
                var indic_4_2_1 = new Indicator()
                {
                    Coefficient = 0.488135593,
                    Subject = "تعداد شاغلین آن حوزه و سهم آن از کل شاغلین کشور",
                    SubCriterion = criterion_4_2_Sub,
                };
                indicators.Add(indic_4_2_1);


                var indic_4_2_2 = new Indicator()
                {
                    Coefficient = 0.511864407,
                    Subject = "تعداد اشتغال ایجاد شده در اثر اجرای برنامه حمایتی ستاد",
                    SubCriterion = criterion_4_2_Sub,
                };
                indicators.Add(indic_4_2_2);



                ////معیارپنجم-زیر معیار اول
                /// 
                var indic_5_1_1 = new Indicator()
                {
                    Coefficient = 0.244127517,
                    Subject = "تعداد پروژه‏ها و حجم انتقال و بومی‌سازی فناوري",
                    SubCriterion = criterion_5_1_Sub,
                };
                indicators.Add(indic_5_1_1);

                var indic_5_1_2 = new Indicator()
                {
                    Coefficient = 0.246979866,
                    Subject = "تعداد ارتباطات سازمان‌یافته بین‌المللی (پروتکل، تفاهم‏نامه و ...) جهت انتقال فناوری",
                    SubCriterion = criterion_5_1_Sub,
                };
                indicators.Add(indic_5_1_2);

                var indic_5_1_3 = new Indicator()
                {
                    Coefficient = 0.23204698,
                    Subject = "تعداد و میزان خرید لیسانس و حق امتیاز (رویالتی) از کشورهای دیگر درآن حوزه",
                    SubCriterion = criterion_5_1_Sub,
                };
                indicators.Add(indic_5_1_3);

                var indic_5_1_4 = new Indicator()
                {
                    Coefficient = 0.276845638,
                    Subject = "تعداد و میزان فروش لیسانس و حق امتیاز (رویالتی) به کشورهای دیگر در آن حوزه",
                    SubCriterion = criterion_5_1_Sub,
                };
                indicators.Add(indic_5_1_4);

                //معیار پنجم- زیر معیار دوم

                var indic_5_2_1 = new Indicator()
                {
                    Coefficient = 0.172225591,
                    Subject = "تعداد محصولات معرفی‌شده در فن بازارها و بورس ایده در آن حوزه",
                    SubCriterion = criterion_5_2_Sub,
                };
                indicators.Add(indic_5_2_1);

                var indic_5_2_2 = new Indicator()
                {
                    Coefficient = 0.172225591,
                    Subject = "تعداد و میزان فروش ساليانه محصولات معرفی‌شده در فن بازارها  و بورس ایده در آن حوزه",
                    SubCriterion = criterion_5_2_Sub,
                };
                indicators.Add(indic_5_2_2);

                var indic_5_2_3 = new Indicator()
                {
                    Coefficient = 0.176470588,
                    Subject = "تعداد دفعات برگزاري فن‏بازارهاي تخصصي فيزيكي و مجازي در آن حوزه",
                    SubCriterion = criterion_5_2_Sub,
                };
                indicators.Add(indic_5_2_3);

                var indic_5_2_4 = new Indicator()
                {
                    Coefficient = 0.188599151,
                    Subject = "تعدا برندهای ایجاد شده در اثر اجرای برنامه حمایتی ستاد",
                    SubCriterion = criterion_5_2_Sub,
                };
                indicators.Add(indic_5_2_4);

                var indic_5_2_5 = new Indicator()
                {
                    Coefficient = 0.14978775,
                    Subject = "تعداد نمایشگاه‌های تخصصی برگزار شده آن حوزه",
                    SubCriterion = criterion_5_2_Sub,
                };
                indicators.Add(indic_5_2_5);

                var indic_5_2_6 = new Indicator()
                {
                    Coefficient = 0.140691328,
                    Subject = "تعداد و میزان حمایت ستاد از برگزاری نمایشگاه‏‏های تخصصی آن حوزه",
                    SubCriterion = criterion_5_2_Sub,
                };
                indicators.Add(indic_5_2_6);

                //معیار پنجم- زیر معیار سوم
                var indic_5_3_1 = new Indicator()
                {
                    Coefficient = 0.511513158
                    ,
                    Subject = "تعداد و میزان خدمات تخصصي تجاری‌سازی ارائه شده",
                    SubCriterion = criterion_5_3_Sub,
                };
                indicators.Add(indic_5_3_1);

                var indic_5_3_2 = new Indicator()
                {
                    Coefficient = 0.488486842,
                    Subject = "تعداد و میزان تسهیلات ارائه شده به منظور تجاری‏سازی دست آوردهای فناورانه",
                    SubCriterion = criterion_5_3_Sub,
                };
                indicators.Add(indic_5_3_2);


                //معیار ششم - زیر معیار اول

                var indic_6_1_1 = new Indicator()
                {
                    Coefficient = 0.247920133,
                    Subject =
                        "تعداد دستاوردهای علمی منتج شده (تولید کالا/ ساخت تجهیزات/ ثبت اختراع/ مقالات ISI و ...) از پروژه‌های تحقیقاتی حمایت‌شده",
                    SubCriterion = criterion_6_1_Sub,
                };
                indicators.Add(indic_6_1_1);

                var indic_6_1_2 = new Indicator()
                {
                    Coefficient = 0.193011647,
                    Subject = "تعداد و میزان حمایت از مجلات علمی-پژوهشی و بین‌المللی معتبر آن حوزه",
                    SubCriterion = criterion_6_1_Sub,
                };
                indicators.Add(indic_6_1_2);

                var indic_6_1_3 = new Indicator()
                {
                    Coefficient = 0.167221298,
                    Subject = "تعداد و میزان حمایت ستاد از مقالات علمی-پژوهشی و بین‌المللی معتبر آن حوزه",
                    SubCriterion = criterion_6_1_Sub,
                };
                indicators.Add(indic_6_1_3);

                var indic_6_1_4 = new Indicator()
                {
                    Coefficient = 0.185524126,
                    Subject = "تعداد مقالات علمی-پژوهشی داخلی و بین‌المللی معتبر نمایه شده آن حوزه",
                    SubCriterion = criterion_6_1_Sub,
                };
                indicators.Add(indic_6_1_4);

                var indic_6_1_5 = new Indicator()
                {
                    Coefficient = 0.206322795,
                    Subject = "متوسط ارجاعات مقالات علمی-پژوهشی و بین‌المللی معتبر نمایه شده آن حوزه",
                    SubCriterion = criterion_6_1_Sub,
                };
                indicators.Add(indic_6_1_5);

                //معیار ششم- زیر معیار دوم

                var indic_6_2_1 = new Indicator()
                {
                    Coefficient = 0.356026786,
                    Subject = " تعداد درخواست‏ها و پتنت‏های ثبت شده ملی و بین‌المللی آن حوزه",
                    SubCriterion = criterion_6_2_Sub,
                };
                indicators.Add(indic_6_2_1);

                var indic_6_2_2 = new Indicator()
                {
                    Coefficient = 0.338169643,
                    Subject = "تعداد طرح‌های صنعتی ایرانی ثبت‌شده در کشور",
                    SubCriterion = criterion_6_2_Sub,
                };
                indicators.Add(indic_6_2_2);

                var indic_6_2_3 = new Indicator()
                {
                    Coefficient = 0.305803571,
                    Subject = "تعداد مخترعان کشور در آن حوزه و سهم آن از کل مخترعان کشور",
                    SubCriterion = criterion_6_2_Sub,
                };
                indicators.Add(indic_6_2_3);


                //معیار هفتم - زیر معیار اول

                var indic_7_1_1 = new Indicator()
                {
                    Coefficient = 0.231111111,
                    Subject = " تعداد دفعات برگزاری منظم جلسات کارگروه‌های تخصصی",
                    SubCriterion = criterion_7_1_Sub,
                };
                indicators.Add(indic_7_1_1);

                var indic_7_1_2 = new Indicator()
                {
                    Coefficient = 0.245333333,
                    Subject = "تعداد طرح‏های بررسی شده",
                    SubCriterion = criterion_7_1_Sub,
                };
                indicators.Add(indic_7_1_2);

                var indic_7_1_3 = new Indicator()
                {
                    Coefficient = 0.244444444,
                    Subject = "تعداد طرح‏های معرفی شده به صندوق نوآوری و شکوفایی",
                    SubCriterion = criterion_7_1_Sub,
                };
                indicators.Add(indic_7_1_3);

                var indic_7_1_4 = new Indicator()
                {
                    Coefficient = 0.279111111,
                    Subject = "تعداد معرفی طرح های جسورانه به صندوق های خطر پذیر",
                    SubCriterion = criterion_7_1_Sub,
                };
                indicators.Add(indic_7_1_4);

                foreach (var indicator in indicators)
                {
                    indicator.DeadlinePeriod = randomCoefficient.Next(30, 90);
                }

                indicators.ForEach(n => context.Indicators.AddOrUpdate(n));
                context.SaveChanges();

                #endregion





                //var criteria = CriterionMockDataGenerate(5, committee, context, user);
                //var subCriterions = SubCriterionMockDataGenerate(criteria, new List<int>() { 3, 4, 3, 2, 2 }, context, user);
                //var numberIndicators = new List<int>();
                //Random random = new Random(DateTime.Now.GetHashCode());
                //for (int i = 0; i < subCriterions.Count; i++)
                //{
                //    numberIndicators.Add(random.Next(5));
                //}
                //var indicators = IndicatorsMockDataGenerate(subCriterions, numberIndicators, context, user);

                //foreach (var indicator in indicators)
                //{
                //    double days = -500;
                //    double? value = null;
                //    bool? moreThan = null;
                //    for (int i = 0; i < 10; i++)
                //    {
                //        var minDateTime = DateTime.Now.AddDays(days);
                //        var idealValue = IndicatorIdealValueMock(indicator, context, minDateTime, moreThan, value, user);
                //        for (int j = 0; j < 10; j++)
                //        {
                //            var realValue = IndicatorRealValueMock(idealValue, context, minDateTime, user);
                //            minDateTime = realValue.Time.GetValueOrDefault();
                //            if (random.Next(0, 10) > 6)
                //            {
                //                break;
                //            }
                //        }
                //        days += 50;
                //        moreThan = idealValue.MoreThan;
                //        value = idealValue.Value + random.Next((int)(-idealValue.Value / 1000), (int)(idealValue.Value / 1000));
                //        if (random.Next(0, 10) > 6)
                //            break;
                //    }

                //}


            }

            private void CreateUser(DataContext context, string roleName, string committeeName, string userName, string password)
            {
                var role = new Role() { Name = roleName };
                context.Roles.AddOrUpdate(role); //add role to db to get id
                context.SaveChanges();
                var committee = new Committee() { Name = committeeName };
                context.Committees.AddOrUpdate(committee); //add committee to get id
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
                context.Users.AddOrUpdate(user);
                context.Roles.AddOrUpdate(role);
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

}
