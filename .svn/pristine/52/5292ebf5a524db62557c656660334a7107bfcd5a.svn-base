DB_BIN_MODE_ARRAY		= 0
DB_BIN_MODE_INT_DICT	= 1
DB_BIN_MODE_STR_DICT	= 2

local DB_BIN_DATATYPE_INT		= 1
local DB_BIN_DATATYPE_BOOL		= 2
local DB_BIN_DATATYPE_FLOAT		= 3
local DB_BIN_DATATYPE_STRING	= 4

local DB_COMPLEX_TYPE_NONE	= 0
local DB_COMPLEX_TYPE_LIST	= 1
local DB_COMPLEX_TYPE_DICT	= 2

Binary = Binary or {}

local function readByte(stream)
	return string.byte(stream:read(1))
end

local function readShort(stream)
	local data = stream:read(2)
	local value = bit.bor(string.byte(data), bit.lshift(string.byte(data, 2), 8))
	return value
end

local function readInt(stream)
	local data = stream:read(4)
	local valueL = bit.bor(string.byte(data), bit.lshift(string.byte(data, 2), 8))
	local valueH = bit.bor(bit.lshift(string.byte(data, 3), 16), bit.lshift(string.byte(data, 4), 24))
	return bit.bor(valueL, valueH)
end

local function readString(stream)
	local length = string.byte(stream:read(1))
	if (length > 0) then
		return stream:read(length)
	else
		return ""
	end
end

local function readBool(stream)
	return (string.byte(stream:read(1)) > 0)
end

local function readFloat(stream)
	local length = string.byte(stream:read(1))
	return tonumber(stream:read(length))
end

local readFuncs = {readInt, readBool, readFloat, readString}

local function readList(stream, dataType)
	local list = {}
	local count = string.byte(stream:read(2))
	local readFunc = readFuncs[dataType]
	for i = 1, count do
		local value = readFunc(stream)
		table.insert(list, value)
	end
	return list
end

local function readDict(stream, dataType)
	local dict = {}
	local count = string.byte(stream:read(2))
	local readFunc = readFuncs[dataType]
	for i = 1, count do
		local key = readString(stream)
		local value = readFunc(stream)
		dict[key] = value
	end
	return dict
end

local function readItem(stream, count)
	local item = {}

	for i = 1, count do
		local name = readString(stream)
		local type = string.byte(stream:read(1))
		local complex = string.byte(stream:read(1))

		if (complex == DB_COMPLEX_TYPE_NONE) then
			local readFunc = readFuncs[type]
			item[name] = readFunc(stream)
		elseif (complex == DB_COMPLEX_TYPE_LIST) then
			item[name] = readList(stream, type)
		elseif (complex == DB_COMPLEX_TYPE_DICT) then
			item[name] = readDict(stream, type)
		end
	end

	return item
end

local function readUnionKeys(stream)
	local unionCount = readByte(stream)
	local keys = {}
	for i = 1, unionCount do
		keys[i] = readString(stream)
	end
	return keys
end

local function createUnionKey(unionKeys, data)
	local values = {}
	for i, uk in ipairs(unionKeys) do
		values[i] = data[uk]
	end
	return table.concat(values, "_")
end

function Binary.loadDB(fileName)
	local stream = assert(io.open(fileName, "rb"))
	local db, ut = Binary.parseDB(stream)
	stream:close()
	return db, ut
end

function Binary.parseDB(stream)
	local dataBase = {}
	local mode = readByte(stream)
	local count = readShort(stream)
	local valueCount = readShort(stream)
	local unionKeys = readUnionKeys(stream)
	local unionTable = {}
	local existUnion = (#unionKeys > 0)

	if (mode == DB_BIN_MODE_ARRAY) then
		for i = 1, count do
			local item = readItem(stream, valueCount)
			table.insert(dataBase, item)
			if existUnion then
				local key = createUnionKey(unionKeys, item)
				unionTable[key] = item
			end
		end
	elseif (mode == DB_BIN_MODE_INT_DICT) then
		for i = 1, count do
			local index = stream:read(4)
			local item = readItem(stream, valueCount)
			dataBase[index] = item
			if existUnion then
				local key = createUnionKey(unionKeys, item)
				unionTable[key] = item
			end
		end
	elseif (mode == DB_BIN_MODE_STR_DICT) then
		for i = 1, count do
			local key = readString(stream)
			local item = readItem(stream, valueCount)
			dataBase[key] = item
			if existUnion then
				local key = createUnionKey(unionKeys, item)
				unionTable[key] = item
			end
		end
	end

	return dataBase, unionTable
end