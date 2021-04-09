#include <iostream>
#include <chrono>
#include <vector>
#include <ppl.h>
#include "PolynomialGenerator.h"
#include <functional>

using namespace concurrency;

int task(std::vector<Polynomial>& list,
	std::function<double(std::vector<Polynomial>)> calc);

std::vector<Polynomial> generateList(size_t size)
{
	PolynomialGenerator generator;

	auto beginl = std::chrono::steady_clock::now();

	std::vector<Polynomial> list = generator.generateList(size);

	auto endl = std::chrono::steady_clock::now();
	auto elapsedl = std::chrono::duration_cast<std::chrono::milliseconds>(endl - beginl);
	std::cout << "Time create list: " << elapsedl.count();

	return list;
}

double& operator+=(double& val, vector<double> vec)
{
	for (auto& it : vec)
	{
		val += it;
	}
	return val;
}
int main()
{
	int count;
	std::cin >> count;
	auto list = generateList(count);
	std::cout << "\nParallel";
	task(list, [](std::vector<Polynomial> list) {
		combinable<double> sum;
		parallel_for(0, int(list.size()), [&](int i)
			{
				try
				{
					DiscrimCalc::calc(list[i][0],
									  list[i][1],
									  list[i][2]);
				}
				catch (const std::exception&)
				{

				}
				catch (vector<double> result)
				{
					sum.local() += result;
				}

			});
		return sum.combine(std::plus<double>());
		});
	std::cout << "\nNotParallel";

	task(list, [](std::vector<Polynomial> list) {
		double sum = 0;
		for (int i = 0; i < list.size(); ++i)
		{
			try
			{
				DiscrimCalc::calc(list[i][0],
								  list[i][1],
								  list[i][2]);
			}
			catch (const std::exception&)
			{

			}
			catch (vector<double> result)
			{
				sum += result;
			}

		}
		return sum;
		});
}

int task(std::vector<Polynomial>& list,
	std::function<double(std::vector<Polynomial>)> calc) {

	auto begin = std::chrono::steady_clock::now();
	double sum = calc(list);
	auto end = std::chrono::steady_clock::now();
	auto elapsed = std::chrono::duration_cast<std::chrono::milliseconds>(end - begin);
	std::cout << "\nTime task: " << elapsed.count();
	std::cout << "\nSum: " << sum;
}