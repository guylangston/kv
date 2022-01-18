#!/usr/bin/lua

-- $ sudo luarocks install lunajson
require "luarocks.loader"

local json = require 'lunajson'

-- local jsonraw = '{"test":[1,2,3,4]}'
local jsonraw = io.read("a")
local jsonparse = json.decode( jsonraw )


-- https://github.com/lunarmodules/Penlight
-- luarocks install penlight
require 'pl.pretty'.dump(jsonparse)


--print( jsonparse )
-- print( jsonparse["test"][ 1 ] .. ", " .. jsonparse["test"][ 2 ] .. ", " .. jsonparse["test"][ 3 ] .. ", " .. jsonparse["test"][ 4 ] )
