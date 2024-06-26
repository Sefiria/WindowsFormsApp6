// stdafx.h : include file for standard system include files,
// or project specific include files that are used frequently, but
// are changed infrequently
//

#pragma once

#include "targetver.h"

#include <stdio.h>
#include <tchar.h>

#include <SFML/Window.hpp>
#include <SFML/System.hpp>
#include <SFML/Network.hpp>
#include <SFML/Graphics.hpp>
#include <SFML/Audio.hpp>

#include <iostream>

using namespace std;

#define SCRW 1366
#define SCRH 768
#define VIDSC 1

sf::RenderWindow* GetPtrWindow();

#define iMSP sf::Mouse::getPosition(*GetPtrWindow())
#define MSP fvec(iMSP.x, iMSP.y)
#define MSL sf::Mouse::isButtonPressed(sf::Mouse::Left)
#define MSR sf::Mouse::isButtonPressed(sf::Mouse::Right)
#define KEY(key) sf::Keyboard::isKeyPressed(sf::Keyboard::key)
#define DRW(sp) GetPtrWindow()->draw(sp)
#define SET_VIEW(view) GetPtrWindow()->setView(view)
#define SET_VIEW_DEFAULT GetPtrWindow()->setView(GetPtrWindow()->getDefaultView())
#define OOS(p) (p.x < 0.f || p.y < 0.f || p.x >= SCRW || p.y >= SCRH)
#define IsOnArea(p, area) (p.x < area.left || p.y < area.top || p.x >= area.left + area.width || p.y >= area.top + area.height)
#define DELTATIME (deltaTime.asMicroseconds()/1000.f)
#define PixelsToCoords(p, view) GetPtrWindow()->mapPixelToCoords(vec(p), view)
#define CoordsToPixels(p, view) (fvec)GetPtrWindow()->mapCoordsToPixel(fvec(p), view)

#define LAYER_ENTITY 0
#define LAYER_PARTICLE 1
#define LAYER_COLLISION 2

using fvec = sf::Vector2f;
using  vec = sf::Vector2i;
using ivec = sf::Vector2i;
using  tex = sf::Texture;
using  spr = sf::Sprite;
using  pxl = sf::Uint8;
using  pic = struct { pxl* pixels; int w, h; };
using rect = sf::FloatRect;
using  col = sf::Color;
using Func = void(*)();

class Event
{
private:
	vector<Func> callbacks;

public:
	Event() {};
	~Event() {};

	void operator+=(Func f) { bool exists(false); for (int i = 0; i < callbacks.size() && !exists; i++) if (callbacks[i] == f) exists = true; if (!exists) callbacks.push_back(f); }
	void operator-=(Func f) { for (int i = 0; i < callbacks.size(); i++) if (callbacks[i] == f) { callbacks.erase(callbacks.begin() + i); return; }; }
	void Call() { for (int i = 0; i < callbacks.size(); i++) callbacks[i]();}
};

extern sf::Time deltaTime;
extern float mouseWheelDelta;

#include "Tools.h"
#include "Resources.h"
#include "HButton.h"
#include "Animation.h"

