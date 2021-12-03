#version 330 core

layout (location = 0) in vec3 position;
//layout (location = 1) in vec3 inColor;
//layout (location = 2) in vec3 normal;   

//out ourColor;

uniform mat4 tramsformation;

void main()
{
    gl_Position = tramsformation * vec4(position.x, position.y, position.z, 1.0);
//    ourColor = color;
}