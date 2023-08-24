using ConsoleApp1.Parameters;
using ConsoleApp1.Tasks;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace YandexTask
{
    public  class Complite
    {
        static HttpClient httpClient = new HttpClient();

        private static List<TaskElement> tasksYandex = new();

        public async Task CopliteTaskforYandex()
        {
                using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "https://api.tracker.yandex.net/v2/issues/_search");

                var ContentBody = new ContentRequestForSearchTask
                {
                    filter = new filter
                    {
                        queue = "testqueque",
                        assignee = "EMPTY()"
                    }
                };

                string ContentBodyJson = JsonSerializer.Serialize(ContentBody);

                StringContent stringContent = new StringContent(ContentBodyJson);
                request.Headers.Add("Host", "api.tracker.yandex.net");
                request.Headers.Add("Authorization", "OAuth y0_AgAAAAAhA8v4AApKWAAAAADpZzPrGqwV3Fo4TNy0Y_AK6W6waca29AI");
                request.Headers.Add("X-Cloud-Org-Id", "bpf73mee5do8q3otts73");

                using HttpResponseMessage response = await httpClient.SendAsync(request);
                Console.WriteLine($"Status: {response.StatusCode}\n");
                Console.WriteLine($"Status: {response.Content}\n");

                string responseText = await response.Content.ReadAsStringAsync();
                responseText = responseText.Trim(new char[] { '[', ']' });
                string[] tasks = responseText.Split("},{");

                bool firstEntry = true;
                int CountEntry = 1;
                List<string> entries = new List<string>();


                foreach (string task in tasks)
                {
                    var lokaltask = task;
                    if (firstEntry)
                    {
                        lokaltask = task + "}";
                        entries.Add(lokaltask);
                        CountEntry++;
                        firstEntry = false;
                    }
                    else if (CountEntry == tasks.Length)
                    {
                        lokaltask = "{" + task;
                        entries.Add(lokaltask);
                    }
                    else
                    {
                        lokaltask = "{" + task + "}";
                        entries.Add(lokaltask);
                        CountEntry++;
                    }

                }

                List<TaskElement> tasksList = new List<TaskElement>();
               
                foreach (string task in entries)
                {

                    TaskElement taskElement = JsonSerializer.Deserialize<TaskElement>(task);
                    tasksYandex.Add(taskElement);
                }
        }

        public List<TaskElement> GetTasks()
        {
           List<TaskElement> taskreturn = new List<TaskElement>();

            foreach(var c in tasksYandex)
            {
                taskreturn.Add(c);
            }
            return taskreturn;

        }

        static public async Task GetInfoUserYandexTracker(HttpClient client)
        {
            using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "https://api.tracker.yandex.net/v2/myself");

            request.Headers.Add("Host", "api.tracker.yandex.net");
            request.Headers.Add("Authorization", "OAuth y0_AgAAAAAhA8v4AApKWAAAAADpZzPrGqwV3Fo4TNy0Y_AK6W6waca29AI");
            request.Headers.Add("X-Cloud-Org-Id", "bpf73mee5do8q3otts73");

            using HttpResponseMessage response = await httpClient.SendAsync(request);
            Console.WriteLine($"Status: {response.StatusCode}\n");
            Console.WriteLine($"Status: {response.Content}\n");

            string responseText = await response.Content.ReadAsStringAsync();
            Console.WriteLine(responseText);

        }

        static public async Task SearchTaskYandexTracker(HttpClient client)
        {
            using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "https://api.tracker.yandex.net/v2/issues/_search");

            var ContentBody = new ContentRequestForSearchTask
            {
                filter = new filter
                {
                    queue = "testqueque",
                    assignee = "EMPTY()"
                }
            };

            string ContentBodyJson = JsonSerializer.Serialize(ContentBody);

            StringContent stringContent = new StringContent(ContentBodyJson);
            request.Headers.Add("Host", "api.tracker.yandex.net");
            request.Headers.Add("Authorization", "OAuth y0_AgAAAAAhA8v4AApKWAAAAADpZzPrGqwV3Fo4TNy0Y_AK6W6waca29AI");
            request.Headers.Add("X-Cloud-Org-Id", "bpf73mee5do8q3otts73");

            using HttpResponseMessage response = await httpClient.SendAsync(request);
            Console.WriteLine($"Status: {response.StatusCode}\n");
            Console.WriteLine($"Status: {response.Content}\n");

            string responseText = await response.Content.ReadAsStringAsync();
            Console.WriteLine(responseText);
        }
    }
}
