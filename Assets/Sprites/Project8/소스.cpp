#include<iostream>
#include<functional>
using namespace std;
typedef float n32;
typedef double n64;
typedef int (*func)(int, int);
typedef void (Hero::*func2)(int, int);

void hap1(int x)
{
	cout << x;
}

int hap(int x, int y)
{
	return x + y;
}

int f_cout(void(*f)(int), int y)
{
	f(1);
}

int ReturnX(int x)
{
	//매개변수에서 한번 리턴에서 한번
	return x;
}

int main()
{
#pragma region 멤버함수포인터
	f_cout(hap1, 5);

	//int(*f)(int, int) = &hap;
	func f = &hap;
	function<int(int, int)> f = hap;
	cout << f(1, 2) << endl;

	n32 a;
	n64 b;

	cout << typeid(a).name() << endl;
	cout << typeid(b).name() << endl;

	void (Hero:: * f2)(int, int) = Hero::hap;
	//f2(1,2) 에러

	Hero hero;
	(hero.*f2)(1, 2);

	/*func2 f2222 = &Hero::hap;
	(hero.*f2222)(1, 2);*/

	function<void(Hero*, int, int)> func_2 = &Hero::hap;
	func_2(&hero, 1, 2);

#pragma endregion

	int a = 1;
	int b = a;

	int a = 1;
	int b;
	b = a;

	func(a);

	Heros hero0{ 50.0f, 100.0f, "Korea Hong Kil Dong" };
	Heros hero1 = hero0;

	Heros h2;
	h2 = hero0;

	hero1.printHero();
	h2.printHero();

	Vec v0{ 0,1,2 };
	Vec v1{ 1,2,3 };

	Vec v2 = v0 + v1;
	Vec v3 = operator*(3.0f, v0);

	v0.printVec();
	v1.printVec();
	v2.printVec();
	v3.printVec();

	VecA va{ 0,1,2 };
	VecB vb1{ 1,2,3 };

	VecB vb2 = vb1 + va;

	vb2.print();
}

class Hero
{
public:
	void hap(int x, int y)
	{
		cout << x + y << endl;
	}
};

class Heros
{
private:
	float hp, pow;
	char* name;

public:
	Heros() {}
	Heros(float hp, float pow, const char* name)
		:hp(hp), pow(pow)
	{
		name = new char[strlen(name) + 1];
		strcpy(this->name, name);
	}

	Heros(const Heros& hero)
		:Heros(hero.hp, hero.pow, hero.name)
	{
		strcpy(this->name, hero.name);
	}

	Heros& operator=(const Heros& hero)
	{
		this->hp = hero.hp;
		this->pow = hero.pow;
		delete[] this->name;

		this->name = new char[strlen(hero.name) + 1];
		strcpy(this->name, hero.name);
	}

	~Heros()
	{
		delete[] name;
	}

	void printHero()
	{
		cout << name << endl;
		cout << hp << endl;
		cout << pow << endl;
	}
};

class Vec
{
	//private:
public:

	float x, y, z;

public:

	Vec(float x, float y, float z) :x(x), y(y), z(z)
	{

	}

	Vec operator+(const Vec& v)

	{
		return Vec{ x + v.x, y + v.y, z + v.z };
	}

	Vec operator-()
	{
		return Vec{ x--, y--, z-- };
	}

	void printVec()
	{
		cout << x << endl;
		cout << y << endl;
		cout << z << endl;
	}

	friend Vec operator*(float v0, Vec& v1);
};

class VecA
{
	friend class VecB;
private:
	float x, y, z;

public:
	VecA(float x, float y, float z) :x(x), y(y), z(z) {}
};

class VecB
{
private:
	float x, y, z;

public:
	VecB(float x, float y, float z) :x(x), y(y), z(z) {}

	VecB operator+(const VecA& v)const
	{
		return VecB(x + v.x, y + v.y, z + v.z);
	}

	void print()
	{
		cout << x << endl;
		cout << y << endl;
		cout << z << endl;
	}
};

Vec operator*(float v0, Vec& v1)
{
	return Vec{ v0 + v1.x, v0 + v1.y, v0 + v1.z, };
}