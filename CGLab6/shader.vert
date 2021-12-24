#version 330 core

layout (location = 0) in vec3 position;
layout (location = 1) in vec3 inColor;
layout (location = 2) in vec3 inNormal;

out vec3 fragCoord;
out vec3 normal;
out vec3 color;

uniform mat4 transformation;

uniform bool animate;
uniform uint curTime;
uniform vec3 scale;

void main()
{
    if (animate && inNormal.x > 0){
        vec3 animatedPosition = vec3(sin(float(curTime) / 3000000.0) * scale.x, position.y, position.z);
        gl_Position = transformation * vec4(animatedPosition.x, animatedPosition.y, animatedPosition.z, 1.0);
    } else {
        gl_Position = transformation * vec4(position.x, position.y, position.z, 1.0);
    }

    fragCoord = position;
    color = inColor;
    normal = inNormal;
}