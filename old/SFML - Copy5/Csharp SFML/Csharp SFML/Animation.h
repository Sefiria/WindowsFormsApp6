#pragma once

class Animation_Part;

class Animation_Node
{
public:
	Animation_Node() {}
	~Animation_Node() {}


};

class Animation_Part
{
public:
	Animation_Part() {}
	~Animation_Part() {}

};

class Animation
{
private:
	void Update();
		void UpdateView();
	void Draw();

	float ViewScreenX() { return ViewRectInScreen.left; }
	float ViewScreenY() { return ViewRectInScreen.top; }
	float ViewScreenW() { return ViewRectInScreen.width; }
	float ViewScreenH() { return ViewRectInScreen.height; }
	float ViewScreenContains(fvec pos) { return !OOS(pos) && ViewRectInScreen.contains(pos); }
	float ViewX() { return view.getSize().x*view.getViewport().left; }
	float ViewY() { return view.getSize().y*view.getViewport().top; }
	float ViewW() { return view.getSize().x*(view.getViewport().left + view.getViewport().width); }
	float ViewH() { return view.getSize().y*(view.getViewport().top + view.getViewport().height); }
	fvec ViewPosition() { return fvec(ViewX(), ViewY()); }
	fvec ViewSize() { return fvec(ViewW(), ViewH()); }
	bool ViewContains(fvec p) { return p.x >= ViewX() && p.x < ViewW() && p.y >= ViewY() && p.y < ViewH(); }
	sf::FloatRect ViewRectInScreen;

public:
	Animation();
	~Animation();

	void Animate();
	
	fvec position;
	spr grid;
	sf::RectangleShape ControlArea, PartCreationArea, PartInstantiationArea, NodeInstantiationArea;
	sf::View view;
	fvec MSPInit, MSPLast;
	bool R_MSR;

	HButton test;
};

