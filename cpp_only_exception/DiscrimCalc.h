#pragma once
#include <iostream>
#include <cmath>

using std::vector;

class DiscrimCalc
{
public:
	static void calc(double a, double b, double c)
	{
		if (isEqual(a, 0))
		{
			if (isEqual(b, 0))
			{
				if (isEqual(c, 0))
				{
					throw std::exception("Invalid parametrs");
				}
				else throw vector<double>{};
			}
			else throw vector<double>{-c / b};
		}
		double disc = (b * b) - (4 * a * c);

		if (isEqual(disc, 0))
		{
			throw vector<double>{ (-b) / (2 * a)};
		}
		if (disc < 0)
		{
			throw vector<double>{};
		}
		else {
			throw vector<double>{
				(-b + sqrt(disc)) / (2 * a),
					(-b - sqrt(disc)) / (2 * a)
			};
		}
	}


	static bool isEqual(double a, double b, double eps = 0.0000000001)
	{
		return abs(a - b) < eps;
	}
};

