using BlogSite.Data;
using BlogSite.Middleware;
using BlogSite.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

internal static partial class Program
{
	private static void Main(string[] args)
	{
		var builder = WebApplication.CreateBuilder(args);

		builder.WebHost.UseUrls("http://localhost:54321"
#if DEBUG
			, "https://localhost:12345"
#endif
		);
		builder.Services.AddRazorPages();
		builder.Services.AddServerSideBlazor();
		builder.Services.AddSingleton<Database>();
		builder.Services.AddSingleton<BlogPostLoader>();
		builder.Services.AddTransient<BlogUrlMiddleware>();
		builder.WebHost.UseStaticWebAssets();
		var app = builder.Build();
		app.UseForwardedHeaders(new ForwardedHeadersOptions
		{
			ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
		});

		app.UseStaticFiles();
		app.UseMiddleware<BlogUrlMiddleware>();
		app.UseRouting();
		app.MapBlazorHub();
		app.MapFallbackToPage("/_Host");
		app.Run();
	}

	[GeneratedRegex(@"(?<=[^0-9-_])([A-Z0-9])", RegexOptions.Compiled)]
	public static partial Regex PascalCaseRegex();

	public static string Humanize(this string pascalCaseString)
	{
		return PascalCaseRegex().Replace(pascalCaseString, x =>
		{
			return x.Result(" $1").ToLower();
		});
	}

	public static string Browserize(this string humanString)
	{
		var stringBuilder = new StringBuilder(humanString);
		stringBuilder.Replace("-", " minus ")
			.Replace("+", " plus ")
			.Replace('.', ' ')
			.Replace(' ', '-');
		return HttpUtility.UrlEncode(stringBuilder.ToString().Trim('-'));
	}

	public static int MonthLength(this DateTime dateTime)
	{
		return DateTime.DaysInMonth(dateTime.Year, dateTime.Month);
	}

	public static int YearLength(this DateTime dateTime)
	{
		return DateTime.IsLeapYear(dateTime.Year) ? 366 : 365;
	}

	public static string GetUriString(this BlogPost post)
	{
		return $"/{post.PublishedOn.Year}/{post.PublishedOn.Month}/{post.PublishedOn.Day}/{post.Id}/{post.Title.Browserize()}";
	}
	const string lettersString = "ABCDEFGHIJKLMNOPQRSTUVWXYZaaabcdeeeefghiijklmnooopqrstuuvwxyzaaabcdeeeefghiijklmnooopqrstuuvwxyzaaabcdeeeefghiijklmnooopqrstuuvwxyzaaabcdeeeefghiijklmnooopqrstuuvwxyzaaabcdeeeefghiijklmnooopqrstuuvwxyzaaabcdeeeefghiijklmnooopqrstuuvwxyzaaabcdeeeefghiijklmnooopqrstuuvwxyzaaabcdeeeefghiijklmnooopqrstuuvwxyzaaabcdeeeefghiijklmnooopqrstuuvwxyzaaabcdeeeefghiijklmnooopqrstuuvwxyzaaabcdeeeefghiijklmnooopqrstuuvwxyzaaabcdeeeefghiijklmnooopqrstuuvwxyzaaabcdeeeefghiijklmnooopqrstuuvwxyzaaabcdeeeefghiijklmnooopqrstuuvwxyzaaabcdeeeefghiijklmnooopqrstuuvwxyzaaabcdeeeefghiijklmnooopqrstuuvwxyzaaabcdeeeefghiijklmnooopqrstuuvwxyzaaabcdeeeefghiijklmnooopqrstuuvwxyz";
	
	public static Paragraph Paragraph(this Random random, int minWordCount = 50, int maxWordCount = 200)
	{
		var paragraph = new Paragraph(null, string.Join(' ', random.Words(minWordCount, maxWordCount)) + ".");
		if (Random.Shared.NextDouble() > .5)
		{
			paragraph.Title = string.Join(' ', random.Words(2, 5));
		}
		return paragraph;
	}
	public static Paragraph[] Paragraphs(this Random random, int minParagraphCount = 2, int maxParagraphCount = 20)
	{
		Paragraph[] paragraphs = new Paragraph[random.Next(minParagraphCount, maxParagraphCount)];
		for (int i = 0; i < paragraphs.Length; i++)
		{
			paragraphs[i] = random.Paragraph();
		}
		return paragraphs;
	}

	public static string Word(this Random random)
	{
		char[] characters = new char[random.Next(2, 7)];
		for (int i = 0; i < characters.Length; i++)
		{
			characters[i] = lettersString[Random.Shared.Next(0, lettersString.Length)];
		}
		return new string(characters);
	}


	public static string[] Words(this Random random, int minCount, int maxCount)
	{
		string[] words = new string[random.Next(minCount, maxCount)];
		for (int i = 0; i < words.Length; i++)
		{
			words[i] = random.Word();
		}
		return words;
	}
	public static string DeAnchorize(this string url)
	{
		return new string(url.TakeWhile(x => x != '#').ToArray());
	}

	public static TimeSpan CalculateReadingTime(this BlogPost blogPost)
	{
		return blogPost.ToString().CalculateReadingTime();
	}

	public static TimeSpan CalculateReadingTime(this string text)
	{
		const decimal wordsPerSecond = 225/60;
		int words = text.Count(x => x == ' ') + 1;
		int secondsToRead = decimal.ToInt32(words/wordsPerSecond);
		return TimeSpan.FromSeconds(secondsToRead);
	}

	public static void CalculateDate(ref int year, ref int month, ref int day)
	{
		while (month > 12)
		{
			month -= 12;
			year++;
		}
		int daysInMonth = DateTime.DaysInMonth(year, month);
		while (day > daysInMonth)
		{
			day -= daysInMonth;
			month++;
			if (month > 12)
			{
				month -= 12;
				year++;
			}
		}
	}

	public static void Reload(this NavigationManager navigation)
	{
		navigation.NavigateTo(navigation.Uri, true);
	}
}