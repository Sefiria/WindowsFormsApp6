#pragma once

#define PI 3.14159265359f

namespace NTools
{
	void LoadResource(spr& sp, pic picture);
	void LoadImage(spr& sp, sf::Image img);
	fvec SprSize(spr sp);
	float SprW(spr sp);
	float SprH(spr sp);
}

static class Tools
{
public:
	static sf::Uint8* GetDecodedPixels(pxl* codedPixels, int size);
	static void Normalize(fvec& look);
	static fvec GetNormalized(fvec look);
	static float Distance(fvec A, fvec B);
	static fvec Distance2(fvec A, fvec B);
	static bool fvecEqual(fvec A, fvec B, float epsilon = 0.00001f);
	static fvec Vec(fvec A, fvec B);
	static bool CollisionAABB(fvec Ap, ivec As, fvec Bp, ivec Bs);
	static bool CollisionAABB(rect A, rect B);
	template<class T>
	static inline T abs(T _a)
	{
		return _a < 0 ? -_a : _a;
	}
	static float ToRad(float deg);
	static float ToDeg(float rad);
	static sf::Vector2f GetLocalSize(const sf::Text& text);

};