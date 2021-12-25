#version 330 core

const int Red = 0;
const int Blue = 1;
const int Black = 2;

out vec4 color;

uniform int ColorMode;

void main()
{
	if (ColorMode == Red)
		color = vec4(0.7f, 0.0f, 0.0f, 1.0f);
	if (ColorMode == Blue)
		color = vec4(0.0f, 0.0f, 0.7f, 1.0f);
    if (ColorMode == Black)
        color = vec4(0.0f, 0.0f, 0.0f, 1.0f);
}