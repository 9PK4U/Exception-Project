#pragma once
#include <ppl.h>
#include <list>
#include <random>
#include <array>
#include "DiscrimCalc.h"
using namespace concurrency;
constexpr int MIN_VALUE_GENERATOR = -1000;
constexpr int MAX_VALUE_GENERATOR = 1000;

using Polynomial = std::array<double, 3>;
class PolynomialGenerator
{
public:
	std::random_device rd;
	std::mt19937 randomizer;
	PolynomialGenerator()
	{
		randomizer = std::mt19937(rd());
	}
	Polynomial generatePolynomial()
	{
		auto _rand = [=]() {return double(randomizer() % (MAX_VALUE_GENERATOR - MIN_VALUE_GENERATOR + 1)) + MIN_VALUE_GENERATOR; };
		return Polynomial{ _rand(),_rand(),_rand()};
	}
	std::vector<Polynomial> generateList(size_t size)
	{

		std::vector<Polynomial> list(size);

		parallel_for_each(list.begin(), list.end(), [&](Polynomial& value)
			{
				value = generatePolynomial();
			});

		return list;
	}
};

