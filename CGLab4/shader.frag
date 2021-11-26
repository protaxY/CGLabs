#version 330 core

const int DarkBlue = 0;
const int Green = 1;
const int Cyan = 2;

out vec4 color;

uniform int ColorMode;

void main()
{
	if (ColorMode == DarkBlue)
		color = vec4(0.0f, 0.2f, 0.4f, 1.0f);
	if (ColorMode == Green)
		color = vec4(0.5f, 1.0f, 0.5f, 1.0f);
    if (ColorMode == Cyan)
        color = vec4(0.0f, 1.0f, 1.0f, 1.0f);
}