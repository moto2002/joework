#import <Foundation/Foundation.h>
#import "DBBase.h"

@implementation DBBase

static Byte* s_data;
static int s_index;

+(void) loadData:(NSString*) fileName
{
    NSString* path = [[NSBundle mainBundle] pathForResource:fileName ofType:DB_BIN_SUFFIX];
    s_data = (Byte*)[[[NSData alloc] initWithContentsOfFile:path] bytes];
    s_index = 0;
}

+(void) clearData
{
    s_data = NULL;
}

+(Byte) readByte
{
    Byte number = s_data[s_index];
    s_index++;
    return number;
}

+(short) readShort
{
    short number = s_data[s_index] | (s_data[s_index + 1] << 8);
    s_index += 2;
    return  number;
}

+(int) readInt
{
    int number = s_data[s_index] | (s_data[s_index + 1] << 8) | (s_data[s_index + 2] << 16) | (s_data[s_index + 3] << 24);
    s_index += 4;
    return number;
}

+(bool) readBool
{
    bool res = (s_data[s_index] > 0);
    s_index++;
    return res;
}

+(NSString*) readString
{
    Byte length = [self readByte];
    Byte* bytes = &s_data[s_index];
    NSString* res = [[NSString alloc] initWithBytes:bytes length:length encoding:NSUTF8StringEncoding];
    s_index += length;
    return res;
}

+(float) readFloat
{
    return [[self readString] floatValue];
}

+(NSArray*) readIntList
{
    short count = [self readShort];
    NSMutableArray* buff = [[NSMutableArray alloc] initWithCapacity:count];
    for (int i = 0; i < count; i++)
    {
        NSNumber* number = [NSNumber numberWithInt:[self readInt]];
        [buff addObject:number];
    }
    return [[NSArray alloc] initWithArray:buff];
}

+(NSArray*) readBoolList
{
    short count = [self readShort];
    NSMutableArray* buff = [[NSMutableArray alloc] initWithCapacity:count];
    for (int i = 0; i < count; i++)
    {
        NSNumber* number = [NSNumber numberWithBool:[self readBool]];
        [buff addObject:number];
    }
    return [[NSArray alloc] initWithArray:buff];
}

+(NSArray*) readFloatList
{
    short count = [self readShort];
    NSMutableArray* buff = [[NSMutableArray alloc] initWithCapacity:count];
    for (int i = 0; i < count; i++)
    {
        NSNumber* number = [NSNumber numberWithFloat:[self readFloat]];
        [buff addObject:number];
    }
    return [[NSArray alloc] initWithArray:buff];
}

+(NSArray*) readStringList
{
    short count = [self readShort];
    NSMutableArray* buff = [[NSMutableArray alloc] initWithCapacity:count];
    for (int i = 0; i < count; i++)
    {
        NSString* text = [self readString];
        [buff addObject:text];
    }
    return [[NSArray alloc] initWithArray:buff];
}

+(NSDictionary*) readIntDict
{
    short count = [self readShort];
    NSMutableDictionary* buff = [[NSMutableDictionary alloc] initWithCapacity:count];
    for (int i = 0; i < count; i++)
    {
        NSString* key = [self readString];
        NSNumber* number = [NSNumber numberWithInt:[self readInt]];
        [buff setObject:number forKey:key];
    }
    return [[NSDictionary alloc] initWithDictionary:buff];
}

+(NSDictionary*) readBoolDict
{
    short count = [self readShort];
    NSMutableDictionary* buff = [[NSMutableDictionary alloc] initWithCapacity:count];
    for (int i = 0; i < count; i++)
    {
        NSString* key = [self readString];
        NSNumber* number = [NSNumber numberWithBool:[self readBool]];
        [buff setObject:number forKey:key];
    }
    return [[NSDictionary alloc] initWithDictionary:buff];
}

+(NSDictionary*) readFloatDict
{
    short count = [self readShort];
    NSMutableDictionary* buff = [[NSMutableDictionary alloc] initWithCapacity:count];
    for (int i = 0; i < count; i++)
    {
        NSString* key = [self readString];
        NSNumber* number = [NSNumber numberWithFloat:[self readFloat]];
        [buff setObject:number forKey:key];
    }
    return [[NSDictionary alloc] initWithDictionary:buff];
}

+(NSDictionary*) readStringDict
{
    short count = [self readShort];
    NSMutableDictionary* buff = [[NSMutableDictionary alloc] initWithCapacity:count];
    for (int i = 0; i < count; i++)
    {
        NSString* key = [self readString];
        NSString* text = [self readString];
        [buff setObject:text forKey:key];
    }
    return [[NSDictionary alloc] initWithDictionary:buff];
}

@end