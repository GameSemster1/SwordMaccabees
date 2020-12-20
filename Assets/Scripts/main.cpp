#include <iostream>
#include <vector>
#include <algorithm>
#include "alg.h"

using namespace std;

int main(){


/*
	/////////////////////////////////
	////////// FIBONNACCI ///////////
	/////////////////////////////////

	int myCheck[] = {2,3,5,8,13,21};
	bool f = Fib(begin(myCheck), end(myCheck));
	cout << boolalpha << (f == true) << endl;

	int ia1[] = {21,34,55};
	bool f1 = Fib(begin(ia1), end(ia1));
	cout << boolalpha << (f1 == true) << endl;

	int ia2[] = {3,4,7};
	bool f2 = Fib(begin(ia2), end(ia2));
	cout << boolalpha << (f2 == false) << endl;

	int ia3[] = {144};
	bool f3 = Fib(begin(ia3), end(ia3));
	cout << boolalpha << (f3 == true) << endl;
*/
	////////////////////////////////
	////////// TRANSPOSE ///////////
	////////////////////////////////

	// int ia4[] = {1,2,3,4,5,6,7,8};
	// int * p1 = Transpose(begin(ia4), end(ia4));
	// for(int i=0; i < 8; i++){
	// 	cout << ia4[i] << " ";
	// }
	// cout << endl;

	// int ia5[] = {1,2,3,4,5,6,7};
	// int * p2 = Transpose(begin(ia5), end(ia5));
	// for(int i=0; i < 7; i++){
	// 	cout << ia5[i] << " ";
	// }
	// cout << endl;

	//////////////////////////////////
	////////// TRANSFORM_2 ///////////
	//////////////////////////////////

	int ia6[] =  {1,2,3,10,8,6,12,2,5};
	vector<int> vec(100);
	auto tp = Transform2(begin(ia6), end(ia6), vec.begin(),
				[](const int x, const int y){return x + y;});

	[](const int x, const int y){return x + y;};

	// {3,13,14,14,5,.. // vec
	cout << endl;

	int ia7[] = {1,2,3,10,8,6,12,2};
	vector<int> vec2(100);
	auto tp2 = Transform2(begin(ia7), end(ia7), vec2.begin(),
				[](const int x, const int y){return x * y;});

	[](const int x, const int y){return x * y;};

	// {2,30,48,24,.. // vec2
	cout << endl;


	
	return 0;
}