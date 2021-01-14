using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace hashcode_2021_practice
{
	class Pizza
	{
		public int Index { get; set; }
		public List<string> Ingredients { get; set; }
		public bool Delivered { get; set; }
	}

	class Program
	{
		static void Main(string[] args)
		{
			string[] inputFiles = {
				"a_example",
				"b_little_bit_of_everything.in",
				"c_many_ingredients.in",
				"d_many_pizzas.in",
				"e_many_teams.in"
			};

			foreach (var inputFile in inputFiles)
			{
				var pizzas = new List<Pizza>();

				StreamReader sr = new StreamReader(Path.Combine(path, inputFile));

				var row = sr.ReadLine().Split(' ');

				var pizzaCount = int.Parse(row[0]);
				var teams2 = int.Parse(row[1]);
				var teams3 = int.Parse(row[2]);
				var teams4 = int.Parse(row[3]);

				for (int i = 0; i < pizzaCount; i++)
				{
					row = sr.ReadLine().Split(' ');

					pizzas.Add(new Pizza()
					{
						Index = i,
						Ingredients = row.Skip(1).OrderBy(x => x).ToList(),
						Delivered = false
					});
				}

				Solve(pizzaCount, teams2, teams3, teams4, pizzas);
			}
		}

		class Calculon
		{
			public double Score { get; set; }
			public List<string> Ingredients { get; set; }
			public int[] PizzaIndexes { get; set; }
		}


		// static IEnumerable<IEnumerable<T>> GetPermutations<T>(IEnumerable<T> list, int length)
		// {
		// 	if (length == 1) return list.Select(t => new T[] { t });
		// 	return GetPermutations(list, length - 1)
		// 		.SelectMany(t => list.Where(o => !t.Contains(o)),
		// 			(t1, t2) => t1.Concat(new T[] { t2 }));
		// }

		static IEnumerable<IEnumerable<T>> GetPermutations<T>(IEnumerable<T> items, int count)
		{
			int i = 0;
			foreach (var item in items)
			{
				if (count == 1)
					yield return new T[] { item };
				else
				{
					foreach (var result in GetPermutations(items.Skip(i + 1), count - 1))
						yield return new T[] { item }.Concat(result);
				}

				++i;
			}
		}

		static List<Calculon> GetBest(List<Pizza> pizzas, int membersCount)
		{
			var result = GetPermutations<int>(pizzas.Select(x => x.Index).ToArray(), membersCount).ToList();

			var calculoni = new List<Calculon>();

			foreach (var r in result)
			{
				var ing = pizzas[r.ToList()[0]].Ingredients.ToArray();
				for (int i = 1; i < membersCount; i++)
				{
					ing = ing.Concat(pizzas[r.ToList()[i]].Ingredients).ToArray();
				}
				ing = ing.Distinct().ToArray();

				var score = Math.Pow(ing.Count(), 2);

				calculoni.Add(new Calculon()
				{
					Score = score,
					Ingredients = ing.ToList(),
					PizzaIndexes = r.ToArray()
				});
			}

			return calculoni.OrderByDescending(x => x.Score).ToList();
		}

		static void Solve(int pizzaCount, int teams2, int teams3, int teams4, List<Pizza> pizzas)
		{
			var result2 = GetBest(pizzas, 2);
			var result3 = GetBest(pizzas, 3);
			var result4 = GetBest(pizzas, 4);
		}
	}
}
