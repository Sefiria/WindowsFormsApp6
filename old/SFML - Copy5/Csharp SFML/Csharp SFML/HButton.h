#pragma once

class HButton
{
public:
	enum Alignment { Top, Left, Center, Right, Bottom };

private:
	spr sp_idle, sp_focused, sp_down, sp_disabled, sp_dragdrop;
	col col_idle, col_focused, col_down, col_disabled;
	float radius;

	void CreateSprite(spr& sp, col c, col bound = col::White, bool fill = true);
	void SetTextLocalPosition();

	bool dropping;
	bool R_MSL;

	spr* sp;
	string Text;
	sf::Text txt;
	fvec textLocalPosition;
	Alignment TextAlignment;
	sf::Font arial;

public:
	HButton(fvec position = fvec(0,0), float radius = 8.f);
	~HButton();

	void Update();
	void Draw();
	void SetText(string text);
	void SetTextAlignment(Alignment TextAlignment);
	void SetTextCharacterSize(unsigned int size);

	fvec position;
	bool Enabled;
	bool AllowUserToMoveWhenDisabled;
	col ForegroundColor, BackgroundColor;
	fvec textOffset;
	sf::IntRect AllowedArea;

	Event OnClick;
	Event OnDragDrop;
};

