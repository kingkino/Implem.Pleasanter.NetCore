use [Implem.Pleasanter];
declare @_U int;                                  set @_U = '1';
declare @_D int;                                  set @_D = '0';
declare @ReferenceId1 bigint;                     set @ReferenceId1 = '1';
declare @Word_Param2 nvarchar(5);                 set @Word_Param2 = 'linux';
declare @Word_Param3 nvarchar(4);                 set @Word_Param3 = 'アルファ';
declare @Word_Param4 nvarchar(1);                 set @Word_Param4 = '版';
declare @Word_Param5 nvarchar(1);                 set @Word_Param5 = '1';
declare @Word_Param6 nvarchar(4);                 set @Word_Param6 = '2018';
declare @Word_Param7 nvarchar(1);                 set @Word_Param7 = '/';
declare @Word_Param8 nvarchar(2);                 set @Word_Param8 = '08';
declare @Word_Param9 nvarchar(2);                 set @Word_Param9 = '14';
declare @Word_Param10 nvarchar(2);                set @Word_Param10 = '15';
declare @Word_Param11 nvarchar(1);                set @Word_Param11 = ':';
declare @Word_Param12 nvarchar(2);                set @Word_Param12 = '06';
declare @Word_Param13 nvarchar(2);                set @Word_Param13 = '53';
declare @Word_Param14 nvarchar(13);               set @Word_Param14 = 'administrator';

begin try

begin transaction;
delete 
from [SearchIndexes] 
where ([ReferenceId]=@ReferenceId1) ;
insert into [SearchIndexes]([Creator], [Updator], [Word],
[ReferenceId], [Priority]) 
values(@_U, @_U, @Word_Param2,
1, 4); ;
insert into [SearchIndexes]([Creator], [Updator], [Word],
[ReferenceId], [Priority]) 
values(@_U, @_U, @Word_Param3,
1, 4); ;
insert into [SearchIndexes]([Creator], [Updator], [Word],
[ReferenceId], [Priority]) 
values(@_U, @_U, @Word_Param4,
1, 4); ;
insert into [SearchIndexes]([Creator], [Updator], [Word],
[ReferenceId], [Priority]) 
values(@_U, @_U, @Word_Param5,
1, 1); ;
insert into [SearchIndexes]([Creator], [Updator], [Word],
[ReferenceId], [Priority]) 
values(@_U, @_U, @Word_Param6,
1, 200); ;
insert into [SearchIndexes]([Creator], [Updator], [Word],
[ReferenceId], [Priority]) 
values(@_U, @_U, @Word_Param7,
1, 200); ;
insert into [SearchIndexes]([Creator], [Updator], [Word],
[ReferenceId], [Priority]) 
values(@_U, @_U, @Word_Param8,
1, 200); ;
insert into [SearchIndexes]([Creator], [Updator], [Word],
[ReferenceId], [Priority]) 
values(@_U, @_U, @Word_Param9,
1, 200); ;
insert into [SearchIndexes]([Creator], [Updator], [Word],
[ReferenceId], [Priority]) 
values(@_U, @_U, @Word_Param10,
1, 200); ;
insert into [SearchIndexes]([Creator], [Updator], [Word],
[ReferenceId], [Priority]) 
values(@_U, @_U, @Word_Param11,
1, 200); ;
insert into [SearchIndexes]([Creator], [Updator], [Word],
[ReferenceId], [Priority]) 
values(@_U, @_U, @Word_Param12,
1, 200); ;
insert into [SearchIndexes]([Creator], [Updator], [Word],
[ReferenceId], [Priority]) 
values(@_U, @_U, @Word_Param13,
1, 200); ;
insert into [SearchIndexes]([Creator], [Updator], [Word],
[ReferenceId], [Priority]) 
values(@_U, @_U, @Word_Param14,
1, 100); ;
commit transaction;
end try

begin catch
rollback transaction;
    DECLARE 
        @ErrorMessage    NVARCHAR(4000), 
        @ErrorNumber     INT, 
        @ErrorSeverity   INT, 
        @ErrorState      INT, 
        @ErrorLine       INT, 
        @ErrorProcedure  NVARCHAR(200);

    SELECT 
        @ErrorNumber = ERROR_NUMBER(), 
        @ErrorSeverity = ERROR_SEVERITY(), 
        @ErrorState = ERROR_STATE(), 
        @ErrorLine = ERROR_LINE(), 
        @ErrorProcedure = ISNULL(ERROR_PROCEDURE(),  '-');

    SELECT @ErrorMessage = 
        N'Error %d,  Level %d,  State %d,
 Procedure %s,  Line %d,  ' + 
            'Message: '+ ERROR_MESSAGE();

    RAISERROR 
        (
        @ErrorMessage,  
        16,  
        @ErrorState,                
        @ErrorNumber,     -- parameter: original error number.
        @ErrorSeverity,   -- parameter: original error severity.
        @ErrorState,      -- parameter: original error state.
        @ErrorProcedure,  -- parameter: original error procedure name.
        @ErrorLine       -- parameter: original error line number.
        );
end catch

