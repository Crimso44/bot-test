
export const parseQueryString = (url: string): any => {

    var objURL = {};

    url.replace(
        new RegExp("([^?=&]+)(=([^&]*))?", "g" ),

        function( $0, $1, $2, $3 ): string {
            objURL[ $1 ] = $3;
            return null;
        }
    );

    return objURL;
};