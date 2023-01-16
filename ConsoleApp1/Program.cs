using System;
using System.Text;
using HtmlAgilityPack;
using System.Threading;
using System.Collections;
using static System.Net.Mime.MediaTypeNames;

namespace parsing
{
    class Programm
    {
        static void Main(string[] args)
        {
            HtmlWeb ws = new HtmlWeb();
            ws.OverrideEncoding = Encoding.UTF8;
            HtmlDocument doc = ws.Load("http://bakeevopark.ru/");

            ArrayList list = new ArrayList();
    
            string[] endResult = new string[100];
            DateTime now = DateTime.Today;

            string[] BossMass = new string[100];
            string[] Flat_Num = new string[100];
            string[] Building = new string[100];
            string[] Floor = new string[100];
            string[] Area = new string[100];
            string[] Price = new string[100];



            string str_flat_num ="" ;
            string str_building = "";
            string str_floor = "";

            StreamWriter file = new StreamWriter("TestFile.txt");
            file.Write("flat_id|flat_num|building|floor|area|rooms|price|sprice|status|date");
            


            Console.WriteLine("Подождите пару минут запросы обрататываются....");

            foreach (HtmlNode node in doc.DocumentNode.SelectNodes("//div[@class='flat-items col-md-12']"))
            {
                Console.WriteLine("http://bakeevopark.ru/" + node.GetAttributeValue("href", null));
                list.Add("http://bakeevopark.ru/" + node.GetAttributeValue("href", null));

            }

            foreach (string o in list)
            {
                Thread.Sleep(1000);
                doc = ws.Load(o);
                
                int value;
                
                //дом,этаж,квартира
                foreach (HtmlNode node in doc.DocumentNode.SelectNodes("//div[@class='configurator-title hidden-xs hidden-sm']//a[@href]"))
                {
                    string href = node.GetAttributeValue("href", null);
                    string[] words = href.Split(new char[] { '/', });
                    
               
                    int gount = 1;
                    int str = 0;
                    int str1 = 0;
                    
                    for (int i = 0; i < words.Length; i++)
                    {
                        int.TryParse(string.Join("", words[i].Where(c => char.IsDigit(c))), out value);
                        if(value!=0)
                        {
                            gount++;
                            str++;
                            str1++;
                            endResult[i] = value.ToString();
                            if (gount == 4) 
                            {
                                str_flat_num += endResult[i]+"/";
                                gount = 0;
                            }
                            if(str==1)
                            {
                                str_building += endResult[i] + "/";
                                str = 0;
                            }
                            if(str1 == 2)
                            {
                                str_floor += endResult[i] + "/";
                                str1= 0;
                            }

                        }
   
                    }
                }

                //Площадь
                int count_square = 0;
                
                foreach (HtmlNode link in doc.DocumentNode.SelectNodes("//div[@class='configurator-title hidden-xs hidden-sm']//a"))
                {
                    
                    string line = link.InnerText;
                    line = line.Substring(line.IndexOf('S') + 3);
                    line = line.Trim();
                    Area[count_square] = line;
                    count_square++;
                }
                
                //Цена
                int count_price =0;
                
                foreach (HtmlNode link in doc.DocumentNode.SelectNodes("//div[@class ='flat-item-right-extra-price']"))
                {
                    
                    string line = link.InnerText;
                    line = line.Remove(line.Length - 4);
                    Price[count_price] = line;
                    count_price++;

                }
               

            }


            string[] sort_flat = str_flat_num.Split(new char[] { '/' });
            string[] sort_building = str_building.Split(new char[] { '/' });
            string[] sort_floor = str_floor.Split(new char[] { '/' });

            for (int i = 0; i <100; i++)
            {
                Flat_Num[i]= sort_flat[i];
                Building[i] = sort_building[i];
                Floor[i] = sort_floor[i];
            }

            file.WriteLine("");
            for (int i=0; i <100;i++)
            {
                BossMass[i] = "|" + "|" + Flat_Num[i]+ "|" + Building[i]+ "|" + Floor[i]+ "|" + Area[i]+ "|" + Price[i]+ "|" + now.ToShortDateString()+ "|";
                file.WriteLine(BossMass[i]);
            }
            Console.WriteLine("Текстовик записался,находится в bin/Debug/net6.0/TextFile.txt");
            file.Close();
        }

    }       
 }
    

