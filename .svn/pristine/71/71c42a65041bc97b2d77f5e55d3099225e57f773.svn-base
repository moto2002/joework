DB_TEMPLATE_PLAYER =
{
	Leonard = {name = "Leonard", className = "classHuman", job = "swordMan", jobName = "", str = 16, dex = 12, mind = 8, con = 14, hitPoint = 0, manaPoint = 0, attack = 8, defense = 2, mAttack = 0, speed = 0, movePoint = 5, resist = {3,3,3,3,3}, accuracy = 5, avoid = 1, critical = 10, counter = 4, exp = 100, gold = 200, items = {0,0}, canEquip = {0}, LvMax = 0, face = "Leonard", addPointOrder = {"str","con","dex","mind"}, addPointRate = {1.5,1.3,1.2}}
}

local unionTable = {}

for pk, data in pairs(DB_TEMPLATE_PLAYER) do
	local uk = data.name .. "_" .. data.className
	unionTable[uk] = data
end

function TemplatePlayerGet(name, className)
	return unionTable[name .. "_" .. className]
end