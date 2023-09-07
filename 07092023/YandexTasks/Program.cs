using ConsoleApp1.Parameters;
using ConsoleApp1.Tasks;
using System.Text.Json;

namespace ConsoleApp1
{
    internal class Program
    {
        static HttpClient httpClient = new HttpClient();

        static async Task Main(string[] args)
        {
            /*
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

            //request.Content = stringContent;

            // получаем ответ
            using HttpResponseMessage response = await httpClient.SendAsync(request);
            Console.WriteLine($"Status: {response.StatusCode}\n");
            Console.WriteLine($"Status: {response.Content}\n");

            string responseText = await response.Content.ReadAsStringAsync();
            responseText = responseText.Trim(new char[] {'[',']'});
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
                tasksList.Add(taskElement);
            }

            foreach (TaskElement task in tasksList)
            {
                Console.WriteLine(task.summary + " находится в статусе: " + task.status.display);
            }
            */
        }

        static public async Task GetInfoUserYandexTracker(HttpClient client)
        {
            using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "https://api.tracker.yandex.net/v2/myself");

            request.Headers.Add("Host", "api.tracker.yandex.net");
            request.Headers.Add("Authorization", "OAuth y0_AgAAAAAhA8v4AApKWAAAAADpZzPrGqwV3Fo4TNy0Y_AK6W6waca29AI");
            request.Headers.Add("X-Cloud-Org-Id", "bpf73mee5do8q3otts73");

            // получаем ответ
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

            //request.Content = stringContent;

            // получаем ответ
            using HttpResponseMessage response = await httpClient.SendAsync(request);
            Console.WriteLine($"Status: {response.StatusCode}\n");
            Console.WriteLine($"Status: {response.Content}\n");

            string responseText = await response.Content.ReadAsStringAsync();
            Console.WriteLine(responseText);
        }
    }
}