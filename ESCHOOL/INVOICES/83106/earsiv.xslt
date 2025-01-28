<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="2.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:cac="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2" xmlns:cbc="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2" xmlns:ccts="urn:un:unece:uncefact:documentation:2" xmlns:clm54217="urn:un:unece:uncefact:codelist:specification:54217:2001"	xmlns:clm5639="urn:un:unece:uncefact:codelist:specification:5639:1988"	xmlns:clm66411="urn:un:unece:uncefact:codelist:specification:66411:2001" xmlns:fn="http://www.w3.org/2005/xpath-functions" xmlns:link="http://www.xbrl.org/2003/linkbase"	xmlns:n1="urn:oasis:names:specification:ubl:schema:xsd:Invoice-2"	xmlns:qdt="urn:oasis:names:specification:ubl:schema:xsd:QualifiedDatatypes-2"	xmlns:udt="urn:un:unece:uncefact:data:specification:UnqualifiedDataTypesSchemaModule:2" xmlns:xbrldi="http://xbrl.org/2006/xbrldi" xmlns:xbrli="http://www.xbrl.org/2003/instance"	xmlns:xdt="http://www.w3.org/2005/xpath-datatypes" xmlns:xlink="http://www.w3.org/1999/xlink"	xmlns:xs="http://www.w3.org/2001/XMLSchema" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"	exclude-result-prefixes="cac cbc ccts clm54217 clm5639 clm66411  fn link n1 qdt udt xbrldi xbrli xdt xlink xs xsd xsi">

  <xsl:decimal-format name="european" decimal-separator="," grouping-separator="." NaN=""/>
  <xsl:output version="4.0" method="html" indent="no" encoding="UTF-8" doctype-public="-//W3C//DTD HTML 4.01 Transitional//EN" doctype-system="http://www.w3.org/TR/html4/loose.dtd" use-character-maps="a"/>
  <xsl:param name="SV_OutputFormat" select="'HTML'"/>
  <xsl:variable name="XML" select="/"/>
  <xsl:variable name="DocumentCurrency">
    <xsl:if test="n1:Invoice/cbc:DocumentCurrencyCode = 'TRL' or n1:Invoice/cbc:DocumentCurrencyCode = 'TRY'">
      <xsl:text>TL</xsl:text>
    </xsl:if>
    <xsl:if test="n1:Invoice/cbc:DocumentCurrencyCode != 'TRL' and n1:Invoice/cbc:DocumentCurrencyCode != 'TRY'">
      <xsl:value-of select="n1:Invoice/cbc:DocumentCurrencyCode"/>
    </xsl:if>
  </xsl:variable>
  <xsl:template match="/">
    <html>
      <head>
	    <script type="text/javascript">
             <![CDATA[var QRCode;!function(){function a(a){this.mode=c.MODE_8BIT_BYTE,this.data=a,this.parsedData=[];for(var b=[],d=0,e=this.data.length;e>d;d++){var f=this.data.charCodeAt(d);f>65536?(b[0]=240|(1835008&f)>>>18,b[1]=128|(258048&f)>>>12,b[2]=128|(4032&f)>>>6,b[3]=128|63&f):f>2048?(b[0]=224|(61440&f)>>>12,b[1]=128|(4032&f)>>>6,b[2]=128|63&f):f>128?(b[0]=192|(1984&f)>>>6,b[1]=128|63&f):b[0]=f,this.parsedData=this.parsedData.concat(b)}this.parsedData.length!=this.data.length&&(this.parsedData.unshift(191),this.parsedData.unshift(187),this.parsedData.unshift(239))}function b(a,b){this.typeNumber=a,this.errorCorrectLevel=b,this.modules=null,this.moduleCount=0,this.dataCache=null,this.dataList=[]}function i(a,b){if(void 0==a.length)throw new Error(a.length+"/"+b);for(var c=0;c<a.length&&0==a[c];)c++;this.num=new Array(a.length-c+b);for(var d=0;d<a.length-c;d++)this.num[d]=a[d+c]}function j(a,b){this.totalCount=a,this.dataCount=b}function k(){this.buffer=[],this.length=0}function m(){return"undefined"!=typeof CanvasRenderingContext2D}function n(){var a=!1,b=navigator.userAgent;return/android/i.test(b)&&(a=!0,aMat=b.toString().match(/android ([0-9]\.[0-9])/i),aMat&&aMat[1]&&(a=parseFloat(aMat[1]))),a}function r(a,b){for(var c=1,e=s(a),f=0,g=l.length;g>=f;f++){var h=0;switch(b){case d.L:h=l[f][0];break;case d.M:h=l[f][1];break;case d.Q:h=l[f][2];break;case d.H:h=l[f][3]}if(h>=e)break;c++}if(c>l.length)throw new Error("Too long data");return c}function s(a){var b=encodeURI(a).toString().replace(/\%[0-9a-fA-F]{2}/g,"a");return b.length+(b.length!=a?3:0)}a.prototype={getLength:function(){return this.parsedData.length},write:function(a){for(var b=0,c=this.parsedData.length;c>b;b++)a.put(this.parsedData[b],8)}},b.prototype={addData:function(b){var c=new a(b);this.dataList.push(c),this.dataCache=null},isDark:function(a,b){if(0>a||this.moduleCount<=a||0>b||this.moduleCount<=b)throw new Error(a+","+b);return this.modules[a][b]},getModuleCount:function(){return this.moduleCount},make:function(){this.makeImpl(!1,this.getBestMaskPattern())},makeImpl:function(a,c){this.moduleCount=4*this.typeNumber+17,this.modules=new Array(this.moduleCount);for(var d=0;d<this.moduleCount;d++){this.modules[d]=new Array(this.moduleCount);for(var e=0;e<this.moduleCount;e++)this.modules[d][e]=null}this.setupPositionProbePattern(0,0),this.setupPositionProbePattern(this.moduleCount-7,0),this.setupPositionProbePattern(0,this.moduleCount-7),this.setupPositionAdjustPattern(),this.setupTimingPattern(),this.setupTypeInfo(a,c),this.typeNumber>=7&&this.setupTypeNumber(a),null==this.dataCache&&(this.dataCache=b.createData(this.typeNumber,this.errorCorrectLevel,this.dataList)),this.mapData(this.dataCache,c)},setupPositionProbePattern:function(a,b){for(var c=-1;7>=c;c++)if(!(-1>=a+c||this.moduleCount<=a+c))for(var d=-1;7>=d;d++)-1>=b+d||this.moduleCount<=b+d||(this.modules[a+c][b+d]=c>=0&&6>=c&&(0==d||6==d)||d>=0&&6>=d&&(0==c||6==c)||c>=2&&4>=c&&d>=2&&4>=d?!0:!1)},getBestMaskPattern:function(){for(var a=0,b=0,c=0;8>c;c++){this.makeImpl(!0,c);var d=f.getLostPoint(this);(0==c||a>d)&&(a=d,b=c)}return b},createMovieClip:function(a,b,c){var d=a.createEmptyMovieClip(b,c),e=1;this.make();for(var f=0;f<this.modules.length;f++)for(var g=f*e,h=0;h<this.modules[f].length;h++){var i=h*e,j=this.modules[f][h];j&&(d.beginFill(0,100),d.moveTo(i,g),d.lineTo(i+e,g),d.lineTo(i+e,g+e),d.lineTo(i,g+e),d.endFill())}return d},setupTimingPattern:function(){for(var a=8;a<this.moduleCount-8;a++)null==this.modules[a][6]&&(this.modules[a][6]=0==a%2);for(var b=8;b<this.moduleCount-8;b++)null==this.modules[6][b]&&(this.modules[6][b]=0==b%2)},setupPositionAdjustPattern:function(){for(var a=f.getPatternPosition(this.typeNumber),b=0;b<a.length;b++)for(var c=0;c<a.length;c++){var d=a[b],e=a[c];if(null==this.modules[d][e])for(var g=-2;2>=g;g++)for(var h=-2;2>=h;h++)this.modules[d+g][e+h]=-2==g||2==g||-2==h||2==h||0==g&&0==h?!0:!1}},setupTypeNumber:function(a){for(var b=f.getBCHTypeNumber(this.typeNumber),c=0;18>c;c++){var d=!a&&1==(1&b>>c);this.modules[Math.floor(c/3)][c%3+this.moduleCount-8-3]=d}for(var c=0;18>c;c++){var d=!a&&1==(1&b>>c);this.modules[c%3+this.moduleCount-8-3][Math.floor(c/3)]=d}},setupTypeInfo:function(a,b){for(var c=this.errorCorrectLevel<<3|b,d=f.getBCHTypeInfo(c),e=0;15>e;e++){var g=!a&&1==(1&d>>e);6>e?this.modules[e][8]=g:8>e?this.modules[e+1][8]=g:this.modules[this.moduleCount-15+e][8]=g}for(var e=0;15>e;e++){var g=!a&&1==(1&d>>e);8>e?this.modules[8][this.moduleCount-e-1]=g:9>e?this.modules[8][15-e-1+1]=g:this.modules[8][15-e-1]=g}this.modules[this.moduleCount-8][8]=!a},mapData:function(a,b){for(var c=-1,d=this.moduleCount-1,e=7,g=0,h=this.moduleCount-1;h>0;h-=2)for(6==h&&h--;;){for(var i=0;2>i;i++)if(null==this.modules[d][h-i]){var j=!1;g<a.length&&(j=1==(1&a[g]>>>e));var k=f.getMask(b,d,h-i);k&&(j=!j),this.modules[d][h-i]=j,e--,-1==e&&(g++,e=7)}if(d+=c,0>d||this.moduleCount<=d){d-=c,c=-c;break}}}},b.PAD0=236,b.PAD1=17,b.createData=function(a,c,d){for(var e=j.getRSBlocks(a,c),g=new k,h=0;h<d.length;h++){var i=d[h];g.put(i.mode,4),g.put(i.getLength(),f.getLengthInBits(i.mode,a)),i.write(g)}for(var l=0,h=0;h<e.length;h++)l+=e[h].dataCount;if(g.getLengthInBits()>8*l)throw new Error("code length overflow. ("+g.getLengthInBits()+">"+8*l+")");for(g.getLengthInBits()+4<=8*l&&g.put(0,4);0!=g.getLengthInBits()%8;)g.putBit(!1);for(;;){if(g.getLengthInBits()>=8*l)break;if(g.put(b.PAD0,8),g.getLengthInBits()>=8*l)break;g.put(b.PAD1,8)}return b.createBytes(g,e)},b.createBytes=function(a,b){for(var c=0,d=0,e=0,g=new Array(b.length),h=new Array(b.length),j=0;j<b.length;j++){var k=b[j].dataCount,l=b[j].totalCount-k;d=Math.max(d,k),e=Math.max(e,l),g[j]=new Array(k);for(var m=0;m<g[j].length;m++)g[j][m]=255&a.buffer[m+c];c+=k;var n=f.getErrorCorrectPolynomial(l),o=new i(g[j],n.getLength()-1),p=o.mod(n);h[j]=new Array(n.getLength()-1);for(var m=0;m<h[j].length;m++){var q=m+p.getLength()-h[j].length;h[j][m]=q>=0?p.get(q):0}}for(var r=0,m=0;m<b.length;m++)r+=b[m].totalCount;for(var s=new Array(r),t=0,m=0;d>m;m++)for(var j=0;j<b.length;j++)m<g[j].length&&(s[t++]=g[j][m]);for(var m=0;e>m;m++)for(var j=0;j<b.length;j++)m<h[j].length&&(s[t++]=h[j][m]);return s};for(var c={MODE_NUMBER:1,MODE_ALPHA_NUM:2,MODE_8BIT_BYTE:4,MODE_KANJI:8},d={L:1,M:0,Q:3,H:2},e={PATTERN000:0,PATTERN001:1,PATTERN010:2,PATTERN011:3,PATTERN100:4,PATTERN101:5,PATTERN110:6,PATTERN111:7},f={PATTERN_POSITION_TABLE:[[],[6,18],[6,22],[6,26],[6,30],[6,34],[6,22,38],[6,24,42],[6,26,46],[6,28,50],[6,30,54],[6,32,58],[6,34,62],[6,26,46,66],[6,26,48,70],[6,26,50,74],[6,30,54,78],[6,30,56,82],[6,30,58,86],[6,34,62,90],[6,28,50,72,94],[6,26,50,74,98],[6,30,54,78,102],[6,28,54,80,106],[6,32,58,84,110],[6,30,58,86,114],[6,34,62,90,118],[6,26,50,74,98,122],[6,30,54,78,102,126],[6,26,52,78,104,130],[6,30,56,82,108,134],[6,34,60,86,112,138],[6,30,58,86,114,142],[6,34,62,90,118,146],[6,30,54,78,102,126,150],[6,24,50,76,102,128,154],[6,28,54,80,106,132,158],[6,32,58,84,110,136,162],[6,26,54,82,110,138,166],[6,30,58,86,114,142,170]],G15:1335,G18:7973,G15_MASK:21522,getBCHTypeInfo:function(a){for(var b=a<<10;f.getBCHDigit(b)-f.getBCHDigit(f.G15)>=0;)b^=f.G15<<f.getBCHDigit(b)-f.getBCHDigit(f.G15);return(a<<10|b)^f.G15_MASK},getBCHTypeNumber:function(a){for(var b=a<<12;f.getBCHDigit(b)-f.getBCHDigit(f.G18)>=0;)b^=f.G18<<f.getBCHDigit(b)-f.getBCHDigit(f.G18);return a<<12|b},getBCHDigit:function(a){for(var b=0;0!=a;)b++,a>>>=1;return b},getPatternPosition:function(a){return f.PATTERN_POSITION_TABLE[a-1]},getMask:function(a,b,c){switch(a){case e.PATTERN000:return 0==(b+c)%2;case e.PATTERN001:return 0==b%2;case e.PATTERN010:return 0==c%3;case e.PATTERN011:return 0==(b+c)%3;case e.PATTERN100:return 0==(Math.floor(b/2)+Math.floor(c/3))%2;case e.PATTERN101:return 0==b*c%2+b*c%3;case e.PATTERN110:return 0==(b*c%2+b*c%3)%2;case e.PATTERN111:return 0==(b*c%3+(b+c)%2)%2;default:throw new Error("bad maskPattern:"+a)}},getErrorCorrectPolynomial:function(a){for(var b=new i([1],0),c=0;a>c;c++)b=b.multiply(new i([1,g.gexp(c)],0));return b},getLengthInBits:function(a,b){if(b>=1&&10>b)switch(a){case c.MODE_NUMBER:return 10;case c.MODE_ALPHA_NUM:return 9;case c.MODE_8BIT_BYTE:return 8;case c.MODE_KANJI:return 8;default:throw new Error("mode:"+a)}else if(27>b)switch(a){case c.MODE_NUMBER:return 12;case c.MODE_ALPHA_NUM:return 11;case c.MODE_8BIT_BYTE:return 16;case c.MODE_KANJI:return 10;default:throw new Error("mode:"+a)}else{if(!(41>b))throw new Error("type:"+b);switch(a){case c.MODE_NUMBER:return 14;case c.MODE_ALPHA_NUM:return 13;case c.MODE_8BIT_BYTE:return 16;case c.MODE_KANJI:return 12;default:throw new Error("mode:"+a)}}},getLostPoint:function(a){for(var b=a.getModuleCount(),c=0,d=0;b>d;d++)for(var e=0;b>e;e++){for(var f=0,g=a.isDark(d,e),h=-1;1>=h;h++)if(!(0>d+h||d+h>=b))for(var i=-1;1>=i;i++)0>e+i||e+i>=b||(0!=h||0!=i)&&g==a.isDark(d+h,e+i)&&f++;f>5&&(c+=3+f-5)}for(var d=0;b-1>d;d++)for(var e=0;b-1>e;e++){var j=0;a.isDark(d,e)&&j++,a.isDark(d+1,e)&&j++,a.isDark(d,e+1)&&j++,a.isDark(d+1,e+1)&&j++,(0==j||4==j)&&(c+=3)}for(var d=0;b>d;d++)for(var e=0;b-6>e;e++)a.isDark(d,e)&&!a.isDark(d,e+1)&&a.isDark(d,e+2)&&a.isDark(d,e+3)&&a.isDark(d,e+4)&&!a.isDark(d,e+5)&&a.isDark(d,e+6)&&(c+=40);for(var e=0;b>e;e++)for(var d=0;b-6>d;d++)a.isDark(d,e)&&!a.isDark(d+1,e)&&a.isDark(d+2,e)&&a.isDark(d+3,e)&&a.isDark(d+4,e)&&!a.isDark(d+5,e)&&a.isDark(d+6,e)&&(c+=40);for(var k=0,e=0;b>e;e++)for(var d=0;b>d;d++)a.isDark(d,e)&&k++;var l=Math.abs(100*k/b/b-50)/5;return c+=10*l}},g={glog:function(a){if(1>a)throw new Error("glog("+a+")");return g.LOG_TABLE[a]},gexp:function(a){for(;0>a;)a+=255;for(;a>=256;)a-=255;return g.EXP_TABLE[a]},EXP_TABLE:new Array(256),LOG_TABLE:new Array(256)},h=0;8>h;h++)g.EXP_TABLE[h]=1<<h;for(var h=8;256>h;h++)g.EXP_TABLE[h]=g.EXP_TABLE[h-4]^g.EXP_TABLE[h-5]^g.EXP_TABLE[h-6]^g.EXP_TABLE[h-8];for(var h=0;255>h;h++)g.LOG_TABLE[g.EXP_TABLE[h]]=h;i.prototype={get:function(a){return this.num[a]},getLength:function(){return this.num.length},multiply:function(a){for(var b=new Array(this.getLength()+a.getLength()-1),c=0;c<this.getLength();c++)for(var d=0;d<a.getLength();d++)b[c+d]^=g.gexp(g.glog(this.get(c))+g.glog(a.get(d)));return new i(b,0)},mod:function(a){if(this.getLength()-a.getLength()<0)return this;for(var b=g.glog(this.get(0))-g.glog(a.get(0)),c=new Array(this.getLength()),d=0;d<this.getLength();d++)c[d]=this.get(d);for(var d=0;d<a.getLength();d++)c[d]^=g.gexp(g.glog(a.get(d))+b);return new i(c,0).mod(a)}},j.RS_BLOCK_TABLE=[[1,26,19],[1,26,16],[1,26,13],[1,26,9],[1,44,34],[1,44,28],[1,44,22],[1,44,16],[1,70,55],[1,70,44],[2,35,17],[2,35,13],[1,100,80],[2,50,32],[2,50,24],[4,25,9],[1,134,108],[2,67,43],[2,33,15,2,34,16],[2,33,11,2,34,12],[2,86,68],[4,43,27],[4,43,19],[4,43,15],[2,98,78],[4,49,31],[2,32,14,4,33,15],[4,39,13,1,40,14],[2,121,97],[2,60,38,2,61,39],[4,40,18,2,41,19],[4,40,14,2,41,15],[2,146,116],[3,58,36,2,59,37],[4,36,16,4,37,17],[4,36,12,4,37,13],[2,86,68,2,87,69],[4,69,43,1,70,44],[6,43,19,2,44,20],[6,43,15,2,44,16],[4,101,81],[1,80,50,4,81,51],[4,50,22,4,51,23],[3,36,12,8,37,13],[2,116,92,2,117,93],[6,58,36,2,59,37],[4,46,20,6,47,21],[7,42,14,4,43,15],[4,133,107],[8,59,37,1,60,38],[8,44,20,4,45,21],[12,33,11,4,34,12],[3,145,115,1,146,116],[4,64,40,5,65,41],[11,36,16,5,37,17],[11,36,12,5,37,13],[5,109,87,1,110,88],[5,65,41,5,66,42],[5,54,24,7,55,25],[11,36,12],[5,122,98,1,123,99],[7,73,45,3,74,46],[15,43,19,2,44,20],[3,45,15,13,46,16],[1,135,107,5,136,108],[10,74,46,1,75,47],[1,50,22,15,51,23],[2,42,14,17,43,15],[5,150,120,1,151,121],[9,69,43,4,70,44],[17,50,22,1,51,23],[2,42,14,19,43,15],[3,141,113,4,142,114],[3,70,44,11,71,45],[17,47,21,4,48,22],[9,39,13,16,40,14],[3,135,107,5,136,108],[3,67,41,13,68,42],[15,54,24,5,55,25],[15,43,15,10,44,16],[4,144,116,4,145,117],[17,68,42],[17,50,22,6,51,23],[19,46,16,6,47,17],[2,139,111,7,140,112],[17,74,46],[7,54,24,16,55,25],[34,37,13],[4,151,121,5,152,122],[4,75,47,14,76,48],[11,54,24,14,55,25],[16,45,15,14,46,16],[6,147,117,4,148,118],[6,73,45,14,74,46],[11,54,24,16,55,25],[30,46,16,2,47,17],[8,132,106,4,133,107],[8,75,47,13,76,48],[7,54,24,22,55,25],[22,45,15,13,46,16],[10,142,114,2,143,115],[19,74,46,4,75,47],[28,50,22,6,51,23],[33,46,16,4,47,17],[8,152,122,4,153,123],[22,73,45,3,74,46],[8,53,23,26,54,24],[12,45,15,28,46,16],[3,147,117,10,148,118],[3,73,45,23,74,46],[4,54,24,31,55,25],[11,45,15,31,46,16],[7,146,116,7,147,117],[21,73,45,7,74,46],[1,53,23,37,54,24],[19,45,15,26,46,16],[5,145,115,10,146,116],[19,75,47,10,76,48],[15,54,24,25,55,25],[23,45,15,25,46,16],[13,145,115,3,146,116],[2,74,46,29,75,47],[42,54,24,1,55,25],[23,45,15,28,46,16],[17,145,115],[10,74,46,23,75,47],[10,54,24,35,55,25],[19,45,15,35,46,16],[17,145,115,1,146,116],[14,74,46,21,75,47],[29,54,24,19,55,25],[11,45,15,46,46,16],[13,145,115,6,146,116],[14,74,46,23,75,47],[44,54,24,7,55,25],[59,46,16,1,47,17],[12,151,121,7,152,122],[12,75,47,26,76,48],[39,54,24,14,55,25],[22,45,15,41,46,16],[6,151,121,14,152,122],[6,75,47,34,76,48],[46,54,24,10,55,25],[2,45,15,64,46,16],[17,152,122,4,153,123],[29,74,46,14,75,47],[49,54,24,10,55,25],[24,45,15,46,46,16],[4,152,122,18,153,123],[13,74,46,32,75,47],[48,54,24,14,55,25],[42,45,15,32,46,16],[20,147,117,4,148,118],[40,75,47,7,76,48],[43,54,24,22,55,25],[10,45,15,67,46,16],[19,148,118,6,149,119],[18,75,47,31,76,48],[34,54,24,34,55,25],[20,45,15,61,46,16]],j.getRSBlocks=function(a,b){var c=j.getRsBlockTable(a,b);if(void 0==c)throw new Error("bad rs block @ typeNumber:"+a+"/errorCorrectLevel:"+b);for(var d=c.length/3,e=[],f=0;d>f;f++)for(var g=c[3*f+0],h=c[3*f+1],i=c[3*f+2],k=0;g>k;k++)e.push(new j(h,i));return e},j.getRsBlockTable=function(a,b){switch(b){case d.L:return j.RS_BLOCK_TABLE[4*(a-1)+0];case d.M:return j.RS_BLOCK_TABLE[4*(a-1)+1];case d.Q:return j.RS_BLOCK_TABLE[4*(a-1)+2];case d.H:return j.RS_BLOCK_TABLE[4*(a-1)+3];default:return void 0}},k.prototype={get:function(a){var b=Math.floor(a/8);return 1==(1&this.buffer[b]>>>7-a%8)},put:function(a,b){for(var c=0;b>c;c++)this.putBit(1==(1&a>>>b-c-1))},getLengthInBits:function(){return this.length},putBit:function(a){var b=Math.floor(this.length/8);this.buffer.length<=b&&this.buffer.push(0),a&&(this.buffer[b]|=128>>>this.length%8),this.length++}};var l=[[17,14,11,7],[32,26,20,14],[53,42,32,24],[78,62,46,34],[106,84,60,44],[134,106,74,58],[154,122,86,64],[192,152,108,84],[230,180,130,98],[271,213,151,119],[321,251,177,137],[367,287,203,155],[425,331,241,177],[458,362,258,194],[520,412,292,220],[586,450,322,250],[644,504,364,280],[718,560,394,310],[792,624,442,338],[858,666,482,382],[929,711,509,403],[1003,779,565,439],[1091,857,611,461],[1171,911,661,511],[1273,997,715,535],[1367,1059,751,593],[1465,1125,805,625],[1528,1190,868,658],[1628,1264,908,698],[1732,1370,982,742],[1840,1452,1030,790],[1952,1538,1112,842],[2068,1628,1168,898],[2188,1722,1228,958],[2303,1809,1283,983],[2431,1911,1351,1051],[2563,1989,1423,1093],[2699,2099,1499,1139],[2809,2213,1579,1219],[2953,2331,1663,1273]],o=function(){var a=function(a,b){this._el=a,this._htOption=b};return a.prototype.draw=function(a){function g(a,b){var c=document.createElementNS("http://www.w3.org/2000/svg",a);for(var d in b)b.hasOwnProperty(d)&&c.setAttribute(d,b[d]);return c}var b=this._htOption,c=this._el,d=a.getModuleCount();Math.floor(b.width/d),Math.floor(b.height/d),this.clear();var h=g("svg",{viewBox:"0 0 "+String(d)+" "+String(d),width:"100%",height:"100%",fill:b.colorLight});h.setAttributeNS("http://www.w3.org/2000/xmlns/","xmlns:xlink","http://www.w3.org/1999/xlink"),c.appendChild(h),h.appendChild(g("rect",{fill:b.colorDark,width:"1",height:"1",id:"template"}));for(var i=0;d>i;i++)for(var j=0;d>j;j++)if(a.isDark(i,j)){var k=g("use",{x:String(i),y:String(j)});k.setAttributeNS("http://www.w3.org/1999/xlink","href","#template"),h.appendChild(k)}},a.prototype.clear=function(){for(;this._el.hasChildNodes();)this._el.removeChild(this._el.lastChild)},a}(),p="svg"===document.documentElement.tagName.toLowerCase(),q=p?o:m()?function(){function a(){this._elImage.src=this._elCanvas.toDataURL("image/png"),this._elImage.style.display="block",this._elCanvas.style.display="none"}function d(a,b){var c=this;if(c._fFail=b,c._fSuccess=a,null===c._bSupportDataURI){var d=document.createElement("img"),e=function(){c._bSupportDataURI=!1,c._fFail&&_fFail.call(c)},f=function(){c._bSupportDataURI=!0,c._fSuccess&&c._fSuccess.call(c)};return d.onabort=e,d.onerror=e,d.onload=f,d.src="data:image/gif;base64,iVBORw0KGgoAAAANSUhEUgAAAAUAAAAFCAYAAACNbyblAAAAHElEQVQI12P4//8/w38GIAXDIBKE0DHxgljNBAAO9TXL0Y4OHwAAAABJRU5ErkJggg==",void 0}c._bSupportDataURI===!0&&c._fSuccess?c._fSuccess.call(c):c._bSupportDataURI===!1&&c._fFail&&c._fFail.call(c)}if(this._android&&this._android<=2.1){var b=1/window.devicePixelRatio,c=CanvasRenderingContext2D.prototype.drawImage;CanvasRenderingContext2D.prototype.drawImage=function(a,d,e,f,g,h,i,j){if("nodeName"in a&&/img/i.test(a.nodeName))for(var l=arguments.length-1;l>=1;l--)arguments[l]=arguments[l]*b;else"undefined"==typeof j&&(arguments[1]*=b,arguments[2]*=b,arguments[3]*=b,arguments[4]*=b);c.apply(this,arguments)}}var e=function(a,b){this._bIsPainted=!1,this._android=n(),this._htOption=b,this._elCanvas=document.createElement("canvas"),this._elCanvas.width=b.width,this._elCanvas.height=b.height,a.appendChild(this._elCanvas),this._el=a,this._oContext=this._elCanvas.getContext("2d"),this._bIsPainted=!1,this._elImage=document.createElement("img"),this._elImage.style.display="none",this._el.appendChild(this._elImage),this._bSupportDataURI=null};return e.prototype.draw=function(a){var b=this._elImage,c=this._oContext,d=this._htOption,e=a.getModuleCount(),f=d.width/e,g=d.height/e,h=Math.round(f),i=Math.round(g);b.style.display="none",this.clear();for(var j=0;e>j;j++)for(var k=0;e>k;k++){var l=a.isDark(j,k),m=k*f,n=j*g;c.strokeStyle=l?d.colorDark:d.colorLight,c.lineWidth=1,c.fillStyle=l?d.colorDark:d.colorLight,c.fillRect(m,n,f,g),c.strokeRect(Math.floor(m)+.5,Math.floor(n)+.5,h,i),c.strokeRect(Math.ceil(m)-.5,Math.ceil(n)-.5,h,i)}this._bIsPainted=!0},e.prototype.makeImage=function(){this._bIsPainted&&d.call(this,a)},e.prototype.isPainted=function(){return this._bIsPainted},e.prototype.clear=function(){this._oContext.clearRect(0,0,this._elCanvas.width,this._elCanvas.height),this._bIsPainted=!1},e.prototype.round=function(a){return a?Math.floor(1e3*a)/1e3:a},e}():function(){var a=function(a,b){this._el=a,this._htOption=b};return a.prototype.draw=function(a){for(var b=this._htOption,c=this._el,d=a.getModuleCount(),e=Math.floor(b.width/d),f=Math.floor(b.height/d),g=['<table style="border:0;border-collapse:collapse;">'],h=0;d>h;h++){g.push("<tr>");for(var i=0;d>i;i++)g.push('<td style="border:0;border-collapse:collapse;padding:0;margin:0;width:'+e+"px;height:"+f+"px;background-color:"+(a.isDark(h,i)?b.colorDark:b.colorLight)+';"></td>');g.push("</tr>")}g.push("</table>"),c.innerHTML=g.join("");var j=c.childNodes[0],k=(b.width-j.offsetWidth)/2,l=(b.height-j.offsetHeight)/2;k>0&&l>0&&(j.style.margin=l+"px "+k+"px")},a.prototype.clear=function(){this._el.innerHTML=""},a}();QRCode=function(a,b){if(this._htOption={width:256,height:256,typeNumber:4,colorDark:"#000000",colorLight:"#ffffff",correctLevel:d.H},"string"==typeof b&&(b={text:b}),b)for(var c in b)this._htOption[c]=b[c];"string"==typeof a&&(a=document.getElementById(a)),this._android=n(),this._el=a,this._oQRCode=null,this._oDrawing=new q(this._el,this._htOption),this._htOption.text&&this.makeCode(this._htOption.text)},QRCode.prototype.makeCode=function(a){this._oQRCode=new b(r(a,this._htOption.correctLevel),this._htOption.correctLevel),this._oQRCode.addData(a),this._oQRCode.make(),this._el.title=a,this._oDrawing.draw(this._oQRCode),this.makeImage()},QRCode.prototype.makeImage=function(){"function"==typeof this._oDrawing.makeImage&&(!this._android||this._android>=3)&&this._oDrawing.makeImage()},QRCode.prototype.clear=function(){this._oDrawing.clear()},QRCode.CorrectLevel=d}();]]>	           	
		</script>
        <style type="text/css">
          body {background-color: #FFFFFF;font-family: 'Tahoma', "Times New Roman", Times, serif;font-size: 11px;}
          h1{padding-bottom: 3px;padding-top: 3px;margin-bottom: 5px;text-transform: uppercase;font-family: Arial, Helvetica, sans-serif;font-size: 1.4em;text-transform:none;}
          #lineTable {border-width:1px;border-spacing:;border-style: solid;border-color: black;border-collapse: collapse;background-color:;}
          #lineTable tr {border-width: 1px;padding: 0px;border-style: solid;}
          #lineTable tr td {border-width: 1px;padding: 0px;border-style: solid;}
          #boldTd {font-weight:bold; }
        </style>
        <title>e-Arşiv Fatura</title>
      </head>
      <body style="margin-left=0.6in; margin-right=0.6in; margin-top=0.79in; margin-bottom=0.79in;width:800px;">
        <tr>
          <td>
            <table id ="InvoiceBody">
              <tr>
                <td>
                  <!--fatura kalemlerinin üst kısmı-->
                  <table style="border-color:blue; " border="0" cellspacing="0px" width="800" cellpadding="0px">
                    <tbody>
                      <!--fatura kesen bilgileri, efatura logosu, fatura kesen logosu-->
                      <tr valign="top">
                        <!--faturayı kesen bilgileri-->
                        <td width="40%" style ="font-family: Tahoma; font-size: 9pt;">
                          <hr/>
                          <xsl:for-each select="n1:Invoice/cac:AccountingSupplierParty">
                            <xsl:call-template name="FirmaBilgisi"/>
                          </xsl:for-each>
                        </td>
                        <!--efatura logosu-->
                        <td width="20%" align="center" valign="middle" id="boldTd">
                          <br/>
                          <br/>
                          <img style="width:91px;" align="middle" alt="E-Fatura Logo" src="data:image/jpeg;base64,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" /> 
							<h1 align="center">e-Arşiv Fatura</h1>
                        </td>
                        <!--faturayı kesen firmanın logosu-->
                        <td width="40%" valign="top" align="right">
                          	<div id="qrcode"/>
							<div id="qrvalue" style="visibility: hidden; height: 20px;width: 20px; ; display:none">{"vkntckn":"<xsl:value-of select="n1:Invoice/cac:AccountingSupplierParty/cac:Party/cac:PartyIdentification/cbc:ID[@schemeID = 'TCKN' or @schemeID = 'VKN']"/>","avkntckn":"<xsl:value-of select="n1:Invoice/cac:AccountingCustomerParty/cac:Party/cac:PartyIdentification/cbc:ID[@schemeID = 'TCKN' or @schemeID = 'VKN']"/><xsl:text></xsl:text>","senaryo":"<xsl:value-of select="n1:Invoice/cbc:ProfileID"/>","tip":"<xsl:value-of select="n1:Invoice/cbc:InvoiceTypeCode"/>","tarih":"<xsl:value-of select="n1:Invoice/cbc:IssueDate"/>","no":"<xsl:value-of select="n1:Invoice/cbc:ID"/>","ettn":"<xsl:value-of select="n1:Invoice/cbc:UUID"/>","parabirimi":"<xsl:value-of select="n1:Invoice/cbc:DocumentCurrencyCode"/>","malhizmettoplam":"<xsl:value-of select="n1:Invoice/cac:LegalMonetaryTotal/cbc:LineExtensionAmount"/><xsl:for-each select="n1:Invoice/cac:TaxTotal/cac:TaxSubtotal[cac:TaxCategory/cac:TaxScheme/cbc:TaxTypeCode = '0015']">"<xsl:text>,"kdvmatrah</xsl:text>(<xsl:value-of select="cbc:Percent"/>)":"<xsl:value-of select="cbc:TaxableAmount"/>"</xsl:for-each><xsl:for-each select="n1:Invoice/cac:TaxTotal/cac:TaxSubtotal[cac:TaxCategory/cac:TaxScheme/cbc:TaxTypeCode = '0015']"><xsl:text>,"hesaplanankdv</xsl:text>(<xsl:value-of select="cbc:Percent"/>)":"<xsl:value-of select="cbc:TaxAmount"/>",</xsl:for-each>"vergidahil":"<xsl:value-of select="n1:Invoice/cac:LegalMonetaryTotal/cbc:TaxInclusiveAmount"/>","odenecek":"<xsl:value-of select="n1:Invoice/cac:LegalMonetaryTotal/cbc:PayableAmount"/>"}</div>
							<script type="text/javascript">
								var qrcode = new QRCode(document.getElementById("qrcode"), {
									width : 175,
									height : 175,
									correctLevel : QRCode.CorrectLevel.L
								});

								function makeCode (msg) {		
									var elText = document.getElementById("text");

									qrcode.makeCode(msg);
								}

								makeCode(document.getElementById("qrvalue").innerHTML);
							</script>
                          <br/>
                          <!--#LOGO#-->
                          <div id="dvdFrmLogo"><img src="data:image/jpeg;base64,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" align="right"/></div>
                        </td>
                      </tr>
                      <!--alıcı bilgileri, varsa kaşe imza, faturanumarası olan tablo/despatch table-->
                      <tr valign="top">
                        <!--alıcı bilgileri-->
                        <td align="left" valign="top" style="font-family: Tahoma; font-size: 9pt; " >
                          <hr/>
                          <hr/>
                          <xsl:variable name="Profil" select="//n1:Invoice/cbc:ProfileID"/>
                          <b>
                            <xsl:text>SAYIN</xsl:text>
                          </b>
                          <br/>
                          <xsl:for-each select="n1:Invoice/cac:AccountingCustomerParty">
                            <xsl:call-template name="FirmaBilgisi"/>
                          </xsl:for-each>
                          <br/>
                        </td>
                        <!--gib logosu altında logo istenirse-->
                        <td>
                          <br/>
                          <br/>
                          <!--#KASE#-->
                          <div id="dvdFrmSign"><img src="data:image/jpeg;base64,/9j/4AAQSkZJRgABAQEAYABgAAD/2wBDAAgGBgcGBQgHBwcJCQgKDBQNDAsLDBkSEw8UHRofHh0aHBwgJC4nICIsIxwcKDcpLDAxNDQ0Hyc5PTgyPC4zNDL/2wBDAQkJCQwLDBgNDRgyIRwhMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjL/wAARCABBAJYDASIAAhEBAxEB/8QAHwAAAQUBAQEBAQEAAAAAAAAAAAECAwQFBgcICQoL/8QAtRAAAgEDAwIEAwUFBAQAAAF9AQIDAAQRBRIhMUEGE1FhByJxFDKBkaEII0KxwRVS0fAkM2JyggkKFhcYGRolJicoKSo0NTY3ODk6Q0RFRkdISUpTVFVWV1hZWmNkZWZnaGlqc3R1dnd4eXqDhIWGh4iJipKTlJWWl5iZmqKjpKWmp6ipqrKztLW2t7i5usLDxMXGx8jJytLT1NXW19jZ2uHi4+Tl5ufo6erx8vP09fb3+Pn6/8QAHwEAAwEBAQEBAQEBAQAAAAAAAAECAwQFBgcICQoL/8QAtREAAgECBAQDBAcFBAQAAQJ3AAECAxEEBSExBhJBUQdhcRMiMoEIFEKRobHBCSMzUvAVYnLRChYkNOEl8RcYGRomJygpKjU2Nzg5OkNERUZHSElKU1RVVldYWVpjZGVmZ2hpanN0dXZ3eHl6goOEhYaHiImKkpOUlZaXmJmaoqOkpaanqKmqsrO0tba3uLm6wsPExcbHyMnK0tPU1dbX2Nna4uPk5ebn6Onq8vP09fb3+Pn6/9oADAMBAAIRAxEAPwD3+ikNH50ALVeS6hhljilcK8gJUHuB1qftWDrmny313ZCJmQrvBkA6cdKcUm9RSdi8+s2UahnlwCu4fKeRnFSW+pW10kjxScRcvuXBArHeO8uYfnttjLA0ZULxkHgj2qVrS5jimRYDJNPGi+Z0H0P0rVwjbczUmaTaraLZJd+aDFJwhHVj6CkTU4H0+S82yqiAlg0ZDcdeK5mSe4R/ItbeK5ezkMu5fliUY5GfX2rSsbe/1e0Nxe3csKSn5IocKNvvmh00ldjUmX/7atFXfuYr5Hnlgv8AD7+/tTk1ezfaVkwrQ+cpK8Mvf8RXPaZBqGb/AEsMp8i6Ueew5MZGVBH6VYupxa3Fppclukk0kjFSDgCEcsx9Pp7UcivZC5pGhN4m063jR5naMPsxuXGd3Srp1S1WWWMvhotu7A9elef6hqU2r+JVWOKJrcSJDAj9JG6sf++cmuzGnzQl1jQOEZWjLfxY7E1VSkoJX3YKTZcj1SFwpZJU3MVAdMdBmhtUgGnLfDcYW6YXnrjpVCS/M2o21lLGEuWDPsDZwuMcmljiu1sVs/se3y+Q2/g81HIluPmZc/tWAsFVXLGQxBdvJI7n2rQB4rDW0vo7priBVD+YcozcMh/rW0udoznNRJJbFRbe4+imHOODQGG4LkZI6ZqbMdx9FFFAzCv9VuYNcWzR4kjWHzW3KSWA64qh9vvRE0k10iyzMrpCP4FJIH1rXudHE+rLfi6ljITYyJjDj3qoNF06Gdd0rs0bDaGb7vOcfrW0XCxjJTvoQx3uoO5M06FFn8pQFxkip9K1a4vJZEuCgKRbii9c5P8ATFWjpls8ckBZg0jtKRnkZ70trp1lDP58BJdAQWB9fX8qG4tbDUZJ6sz5fE6iPMMG/GMndgDjJFQajfz6rqS2FpNJbQxQGW5mXrz0Qe/rUuqWtuY47SwGLu5kLxsAD5YP3n+lWToNhDYQRM7KsIP7zdgtn724980e6tQ95la38jyY3jlEMIVo0twNyucZy3qahj1O7kSMRTMQ0vlqFQBmx1x7Vpy6XpZaOVyFRhtRQ2FPGBj3xSz6HpzndKpUs64YMRyOlHPETjIxLZJP7S15ftcpKSwhwoGfu9zXPDUTqHi24llmxbR27vKw4xEBhUB9yDWrcTR20GrQW8TPd312LaMKeTgf0Fc/dx2Vnpb2O8Ge9uB5jKeI4E6/r/OuuFNagyr4ZabUvEP2uUBbW2jkkCDjDuDge2BwK73SNUea/wBRlN3usbFAgVsYL4y3PtxXOeHBbxC5g8pH1O+k3Rx9o0I6n6Vo6V4ePnz6XPFttVuC88pbBuDxhQPT1oryjJvyFFMSxM02uz3Nw7b/ALMJsnjAZuF9sDiuts5fMT7Q1wMMMbCcBao6bbwXd/ql08alZJVjBJ4OwY/nWiLGzJMmxTu5zmuWpO5Vil9tZtJllaX94spU4PIG7FV3u5bbUFU3LHMyqEJ4CY5J9q1RZ2KxSSeWmxzuY9j70k8VksnmzJHmRNpYjqtQpIGmUbiZ/wC0mHnM8E0WY/LfGwgf1qlYXUdvd2E13ctueByZJTwef0rYVLC3mwAinywgXH8NSyabZXKKstvG6gYAI6CnzJKwcrbvct7soCCOaKUIAMY49KKyNRWrnLyC8/tC4RIGZJZEdXz0A610ZpO2efpVRlYmUbnOomoSupMMoEY+/nDyc9Pb1pssF9HEHIdbZF/fDcASMkkiukODxXB+LvEM1loOsXIZmS3VkiVR95uM5+laQfO9CXGxNpllfzD7db70E0ZCAPnCYwq+xreubKb7DZxoryeS6vLGzZMmByMnrWXpGrzz6JAYESPFp5rKE53eoq1c6rMbYxJMMmHmVRjD8dKc1KUthJpKwslrfz36yNbkRq6tGpYbYwByPrmrGsPmwgMqFH89TsBy34ep6VS1LVZljt5dOZrhwuyQ/wAPzD5ST9aytXmewRJGupbnUpYfKgcdBITyAPzojC+472MOOz1dLjVNWihUuzmK3lZ+IJM4Y47k+1RwaTJf61Ho1mhdbSINcyFuSxOTk+5PT2rptWjeLwTNDDcxRyWzL82OjA/xe+eayfBtrJp9gl88plubuU7lzzOR3H0rrU37Nz+RDtc6v+yJ01C3eKKMIhXfID97A7juc9KdcwXVtdXOoYUqAwVPQY+99c0ljeXLJexyOGkVWcOAfkPofpVO4luLnTJ47d5JoHRd8pXoc8gVxu7ZWg7RdKu10u33qozE8mC3V355rSjtrzbCCqx7UAYA/XNOs5ZTqc8TM3krEjIpHC8VWgvC0V19jMrAPgAjLD1bntSbbY9C89tNJYQ27BRggP8AQf4069tXnlhKoGjQEFc4+lUR5sq+ZE8qMsW5y3qR6frV/TJjLaDOSEO0Of4/ek9NQWuhVS1v1SF3CO8TMwUnsRwM1qQGRolMqhXIyQDnBqQciipcrlRikLRRRUlDJCwUlRlscCsGHXbk3EyXFtFFHFOIWff3IzW+wzWXJoNrJ9p8wuwnuBcMM9wMYqotdSZX6DzrNskIlbzBGWChih5z6e1Vbu80m5tJIbqINDJlWVk+8O5/lUn/AAj8RjWN7i4dU4TLfdHoKmGi26wwRoZFMGdjA889fzqvcvclcxl6ckxgmW0nYQW42xkxDLrjgD1qGJ7cpG9+zSphhsRcBMfw7R356V0EGnx27ytE0iiTqueAfUe9Vl0K23b2eV5Mk+Yzc5PenzoLMrPqsVoPKW0WOGMAlSNu1T3/AArKj1WO51NtTaHdCseLAEcHJwW+px+lb13oFpf2zQXvmTqyhXLNywBz296nfSbR8qYflKKoA4ChemPShSiNqRz2vxza5BDb2B2ASb3LDiRemfoDWrJc2+lCzsY0jaQbU9NoPf8AOrSaPbRsSvmA4AX5vujOcD2zU9xYW90UaWIFkIIYDmk5ppLoFnYyJdbksmu2njhKqdsXl5y7Y5Bz6VKmrGRW2RonQQ7xgMe5+nNXRpdsLYwGMsjFmyx5yetQNoFlI7uyyHcMAFvue49OgoTiFpDBqUy3DL5KFI2VJCOpJOOK0lki8141IDJy3FVk0i1W4Wfa5cDBy3De596vbFyTtGT1OOtS2r6DSZlXOpN9muGgMYaMjZuP3vb8egpPtt2m8COJmERcxqfuHHAP61Zn0i0uCxdCCxBO046dKbFo9tDcyXC+YXkGGy3WmnETTKJ1WfzLUBYtskQeRyTtQmrul3M9w84lKOsb7VdeN1SNpNo7IzRfcXaBnjFPstPt7EMIFZQx6E5ptxtoJRlct0UUVmaCN2pD2+tFFADR0/4FTv4/woopiEP3R9acf60UUhjex+tIe/1oooAce/1FOoooAb3ag9/pRRQADqPpTqKKAGD7w+lO7GiigBh7/Wn96KKAFooooA//2Q==" align="right"/></div>
                        </td>
                        <!--fatura bilgileri-->
                        <td align="right" valign="bottom" colspan="2">
                          <table border="1" id="lineTable">
                            <tbody style ="text-align:left;">
                              <tr style="width:105px;">
                                <td id="boldTd">Özelleştirme No:</td>
                                <td>
                                  <xsl:for-each select="n1:Invoice/cbc:CustomizationID">
                                    <xsl:apply-templates/>
                                  </xsl:for-each>
                                </td>
                              </tr>
                              <tr>
                                <td id="boldTd">Fatura Tipi:</td>
                                <td>
                                  <xsl:for-each select="n1:Invoice/cbc:InvoiceTypeCode">
                                    <xsl:apply-templates/>
                                  </xsl:for-each>
                                </td>
                              </tr>
                              <tr>
                                <td id="boldTd">Fatura No:</td>
                                <td>
                                  <xsl:for-each select="n1:Invoice/cbc:ID">
                                    <xsl:apply-templates/>
                                  </xsl:for-each>
                                </td>
                              </tr>
                              <tr>
                                <td id="boldTd">Fatura Tarihi:</td>
                                <td>
                                  <xsl:for-each select="n1:Invoice/cbc:IssueDate">
                                    <xsl:apply-templates select="."/>
                                  </xsl:for-each>
                                </td>
                              </tr>
                              <xsl:if test="n1:Invoice/cbc:IssueTime">
                                <tr>
                                  <td id="boldTd">Fatura Saati:</td>
                                  <td>
                                    <xsl:for-each select="n1:Invoice/cbc:IssueTime">
                                      <xsl:apply-templates select="."/>
                                    </xsl:for-each>
                                  </td>
                                </tr>
                              </xsl:if>
                              <xsl:for-each select="n1:Invoice/cac:DespatchDocumentReference">
                                <tr>
                                  <td id="boldTd">İrsaliye No:</td>
                                  <td>
                                    <xsl:value-of select="cbc:ID"/>
                                  </td>
                                </tr>
                                <tr>
                                  <td id="boldTd">İrsaliye Tarihi</td>
                                  <td>
                                    <xsl:for-each select="cbc:IssueDate">
                                      <xsl:apply-templates select="."/>
                                    </xsl:for-each>
                                  </td>
                                </tr>
                              </xsl:for-each>
                              <xsl:for-each select="//n1:Invoice/cac:OrderReference">
                                <tr>
                                  <td id="boldTd">Sipariş No:</td>
                                  <td>
                                    <xsl:for-each select="cbc:ID">
                                      <xsl:apply-templates/>
                                    </xsl:for-each>
                                  </td>
                                </tr>
                                <tr>
                                  <td id="boldTd">Sipariş Tarihi:</td>
                                  <td>
                                    <xsl:for-each select="cbc:IssueDate">
                                      <xsl:apply-templates select="."/>
                                    </xsl:for-each>
                                  </td>
                                </tr>
                              </xsl:for-each>
                              <xsl:for-each select="n1:Invoice/cac:TaxRepresentativeParty/cac:PartyIdentification/cbc:ID[@schemeID='ARACIKURUMVKN']">
                                <tr>
                                  <td id="boldTd">Aracı Kurum VKN:</td>
                                  <td>
                                    <xsl:value-of select="."/>
                                  </td>
                                </tr>
                                <tr>
                                  <td id="boldTd">Aracı Kurum Unvan:</td>
                                  <td>
                                    <xsl:value-of select="../../cac:PartyName/cbc:Name"/>
                                  </td>
                                </tr>
                              </xsl:for-each>
                              <xsl:for-each select="//n1:Invoice/cac:PaymentMeans/cbc:PaymentDueDate">
                                <tr>
                                  <td id="boldTd">Vade Tarihi:</td>
                                  <td>
                                    <xsl:for-each select="//n1:Invoice/cac:PaymentMeans/cbc:PaymentDueDate">
                                      <xsl:value-of select="substring(.,9,2)"/>-<xsl:value-of select="substring(.,6,2)"/>-<xsl:value-of select="substring(.,1,4)"/>
                                    </xsl:for-each>
                                  </td>
                                </tr>
                              </xsl:for-each>


                              <xsl:if test="$DocumentCurrency != 'TL' and //n1:Invoice/cac:PricingExchangeRate/cbc:CalculationRate">

                                <tr>
                                  <td id="boldTd">
                                    Kur:(<xsl:value-of select = "//n1:Invoice/cac:PricingExchangeRate/cbc:SourceCurrencyCode" />)
                                  </td>
                                  <td>
                                    <xsl:value-of select = "//n1:Invoice/cac:PricingExchangeRate/cbc:CalculationRate" />
                                    <xsl:text> </xsl:text>
                                    <xsl:value-of select = "//n1:Invoice/cac:PricingExchangeRate/cbc:TargetCurrencyCode" />
                                  </td>
                                </tr>
                              </xsl:if>
                            </tbody>
                          </table>
                          <br/>
                        </td>
                      </tr>
                      <!--ettn satırı-->
                      <tr align="left">
                        <td align="left" valign="top">
                          <hr/>
                          <b>ETTN:&#160;</b>
                          <xsl:for-each select="n1:Invoice/cbc:UUID">
                            <xsl:apply-templates/>
                          </xsl:for-each>
                          <br/>
                          <br/>
                        </td>
                      </tr>
                    </tbody>
                  </table>
                  <!--fatura kalemlerinin üst kısmı-->
                </td>
              </tr>
              <tr>
                <td>
                  <!--fatura kalemleri-->
                  <table  border="0" cellspacing="0px" width="800" cellpadding="0px" id="lineTable">
                    <tbody>
                      <tr style ="font-weight:bold;text-align:center;background-color:OldLace;">
                        <td style="width:5px" >
                          <xsl:text>Sıra No</xsl:text>
                        </td>
                        <!--SELLERID1-->
                        <td style="width:250px">
                          <xsl:text>Mal Hizmet</xsl:text>
                        </td>
                        <!--MANUFACTURERID1-->
                        <!--BUYERID1-->
                        <!--DESCRIPTION1-->
                        <!--LINENOTE1-->
                        <td style="width:70px" >
                          <xsl:text>Miktar</xsl:text>
                        </td>
                        <td style="width:70px" >
                          <xsl:text>Birim Fiyat</xsl:text>
                        </td>
						<td style="width: 70px" ><xsl:text>İskonto Oranı</xsl:text></td>
                        <td style="width:70px" ><xsl:text>İskonto Tutarı</xsl:text></td>
                        <td style="width:70px" ><xsl:text>KDV Oranı</xsl:text></td>
                        <td style="width:70px" ><xsl:text>KDV Tutarı</xsl:text></td>
                        <td style="width:30px" ><xsl:text>Diğer Vergiler</xsl:text></td>
                        <td style="width:95px" >
                          <xsl:text>Mal Hizmet Tutarı</xsl:text>
                        </td>
                      </tr>
                      <!--Satır Kalemleri Otomatik Geliyor-->
                      <xsl:for-each select = "//n1:Invoice/cac:InvoiceLine" >
                        <xsl:apply-templates select = "." />
                      </xsl:for-each>
                    </tbody>
                  </table>
                  <!--fatura kalemleri-->
                </td>
              </tr>
              <tr>
                <td>
                  <table style="width:800px;">
                    <tr>
                      <td style="width:500px;">&#160;</td>
                      <td style="width:300px;">
                        <!--dip toplam-->
                        <table style="width:300px;margin-top:10px;margin-bottom:10px;" border ="1" id ="lineTable" align ="right">
                          <tbody>
                            <xsl:if	test="1=1">

                              <tr>
                                <td style="font-weight:bold;width:200px;">&#160;Mal Hizmet Toplam Tutarı</td>
                                <td style="width:100px; text-align:right">
                                  <xsl:for-each select="n1:Invoice/cac:LegalMonetaryTotal/cbc:LineExtensionAmount">
                                    <xsl:call-template name="Curr_Type"/>
                                  </xsl:for-each>
                                </td>
                              </tr>
                              <xsl:for-each select="n1:Invoice/cac:TaxTotal/cac:TaxSubtotal">
                                <xsl:if test="cac:TaxCategory/cac:TaxScheme/cbc:TaxTypeCode = '4171'">
                                  <tr>
                                    <td id="boldTd">&#160;Teslim Bedeli </td>
                                    <td style="text-align:right">
                                      <xsl:for-each select="//n1:Invoice/cac:LegalMonetaryTotal/cbc:LineExtensionAmount">
                                        <xsl:call-template name="Curr_Type"/>
                                      </xsl:for-each>
                                    </td>
                                  </tr>

                                </xsl:if>
                              </xsl:for-each>
                              <tr>
                                <td id="boldTd">&#160;Toplam İskonto</td>
                                <td style="text-align:right">
                                  <xsl:for-each select="n1:Invoice/cac:LegalMonetaryTotal/cbc:AllowanceTotalAmount">
                                    <xsl:call-template name="Curr_Type"/>
                                  </xsl:for-each>
                                </td>
                              </tr>


                              <!--SUBTOTAL1-->
                              <xsl:for-each select="n1:Invoice/cac:TaxTotal/cac:TaxSubtotal">
                                <!--MATRAH1-->
                                <tr>
                                  <td id="boldTd">
                                    &#160;<xsl:text>Hesaplanan </xsl:text>
                                    <xsl:value-of select="cac:TaxCategory/cac:TaxScheme/cbc:Name"/>
                                    <xsl:text>(%</xsl:text>
                                    <xsl:value-of select="cbc:Percent"/>
                                    <xsl:text>)</xsl:text>
                                  </td>
                                  <td style="text-align:right">
                                    <xsl:for-each select="cbc:TaxAmount">
                                      <xsl:call-template name="Curr_Type"/>
                                    </xsl:for-each>
                                  </td>
                                </tr>
                              </xsl:for-each>
                              <xsl:for-each select="n1:Invoice/cac:TaxTotal/cac:TaxSubtotal">
                                <xsl:if test="cac:TaxCategory/cac:TaxScheme/cbc:TaxTypeCode = '4171'">
                                  <tr>
                                    <td id="boldTd">&#160;KDV Matrahı </td>
                                    <td style="text-align:right">

                                      <xsl:value-of
                                          select="format-number(sum(//n1:Invoice/cac:TaxTotal/cac:TaxSubtotal[cac:TaxCategory/cac:TaxScheme/cbc:TaxTypeCode=0015]/cbc:TaxableAmount), '###.##0,00', 'european')"/>
                                      <xsl:if
                                        test="//n1:Invoice/cac:LegalMonetaryTotal/cbc:TaxInclusiveAmount/@currencyID">
                                        <xsl:text> </xsl:text>
                                        <xsl:if
                                          test="//n1:Invoice/cac:LegalMonetaryTotal/cbc:TaxInclusiveAmount/@currencyID = 'TRL' or //n1:Invoice/cac:LegalMonetaryTotal/cbc:TaxInclusiveAmount/@currencyID = 'TRY'">
                                          <xsl:text>TL</xsl:text>
                                        </xsl:if>
                                        <xsl:if
                                          test="//n1:Invoice/cac:LegalMonetaryTotal/cbc:TaxInclusiveAmount/@currencyID != 'TRL' and //n1:Invoice/cac:LegalMonetaryTotal/cbc:TaxInclusiveAmount/@currencyID != 'TRY'">
                                          <xsl:value-of
                                            select="//n1:Invoice/cac:LegalMonetaryTotal/cbc:TaxInclusiveAmount/@currencyID"
											/>
                                        </xsl:if>
                                      </xsl:if>
                                    </td>
                                  </tr>
                                  <tr>
                                    <td id="boldTd">&#160;Tevkifat Dahil Toplam Tutar </td>
                                    <td style="text-align:right">
                                      <xsl:for-each select="//n1:Invoice/cac:LegalMonetaryTotal/cbc:TaxInclusiveAmount">
                                        <xsl:call-template name="Curr_Type"/>
                                      </xsl:for-each>
                                    </td>
                                  </tr>
                                  <tr>
                                    <td id="boldTd">&#160;Tevkifat Hariç Toplam Tutar </td>
                                    <td style="text-align:right">
                                      <xsl:for-each select="//n1:Invoice/cac:LegalMonetaryTotal/cbc:PayableAmount">
                                        <xsl:call-template name="Curr_Type"/>
                                      </xsl:for-each>
                                    </td>
                                  </tr>
                                </xsl:if>
                              </xsl:for-each>
                              <xsl:for-each select="n1:Invoice/cac:WithholdingTaxTotal/cac:TaxSubtotal">
                                <tr>
                                  <td id="boldTd">
                                    &#160;<xsl:text>Hesaplanan KDV Tevkifat</xsl:text>
                                    <xsl:text>(%</xsl:text>
                                    <xsl:value-of select="cbc:Percent"/>
                                    <xsl:text>)</xsl:text>
                                  </td>
                                  <td style="text-align:right">

                                    <xsl:for-each select="cbc:TaxAmount">
                                      <xsl:call-template name="Curr_Type"/>
                                    </xsl:for-each>
                                  </td>
                                </tr>
                              </xsl:for-each>
                              <xsl:if
                                test="sum(n1:Invoice/cac:TaxTotal/cac:TaxSubtotal[cac:TaxCategory/cac:TaxScheme/cbc:TaxTypeCode=9015]/cbc:TaxableAmount)>0">
                                <tr>
                                  <td id="boldTd">&#160;Tevkifata Tabi İşlem Tutarı </td>
                                  <td style="text-align:right">
                                    <xsl:value-of
                                      select="format-number(sum(n1:Invoice/cac:InvoiceLine[cac:TaxTotal/cac:TaxSubtotal/cac:TaxCategory/cac:TaxScheme/cbc:TaxTypeCode=9015]/cbc:LineExtensionAmount), '###.##0,00', 'european')"/>
                                    <xsl:if test="$DocumentCurrency = 'TL'">
                                      <xsl:text>TL</xsl:text>
                                    </xsl:if>
                                    <xsl:if test="$DocumentCurrency != 'TL'">
                                      <xsl:value-of select="$DocumentCurrency"/>
                                    </xsl:if>

                                  </td>
                                </tr>
                                <tr>
                                  <td id="boldTd">&#160;Tevkifata Tabi İşlem Üzerinden Hes. KDV </td>
                                  <td style="text-align:right">
                                    <xsl:value-of
                                                     select="format-number(sum(n1:Invoice/cac:TaxTotal/cac:TaxSubtotal[cac:TaxCategory/cac:TaxScheme/cbc:TaxTypeCode=9015]/cbc:TaxableAmount), '###.##0,00', 'european')"/>
                                    <xsl:if test="$DocumentCurrency = 'TL'">
                                      <xsl:text>TL</xsl:text>
                                    </xsl:if>
                                    <xsl:if test="$DocumentCurrency != 'TL'">
                                      <xsl:value-of select="$DocumentCurrency"/>
                                    </xsl:if>
                                  </td>
                                </tr>
                              </xsl:if>
                              <xsl:if test = "n1:Invoice/cac:InvoiceLine[cac:WithholdingTaxTotal/cac:TaxSubtotal/cac:TaxCategory/cac:TaxScheme]">
                                <tr>
                                  <td id="boldTd">&#160;Tevkifata Tabi İşlem Tutarı </td>
                                  <td style="text-align:right">
                                    <xsl:if test = "n1:Invoice/cac:InvoiceLine[cac:WithholdingTaxTotal/cac:TaxSubtotal/cac:TaxCategory/cac:TaxScheme]">
                                      <xsl:value-of
                                        select="format-number(sum(n1:Invoice/cac:InvoiceLine[cac:WithholdingTaxTotal/cac:TaxSubtotal/cac:TaxCategory/cac:TaxScheme]/cbc:LineExtensionAmount), '###.##0,00', 'european')"/>
                                    </xsl:if>
                                    <xsl:if test = "//n1:Invoice/cac:TaxTotal/cac:TaxSubtotal/cac:TaxCategory/cac:TaxScheme/cbc:TaxTypeCode=&apos;9015&apos;">
                                      <xsl:value-of
                                        select="format-number(sum(n1:Invoice/cac:InvoiceLine[cac:TaxTotal/cac:TaxSubtotal/cac:TaxCategory/cac:TaxScheme/cbc:TaxTypeCode=9015]/cbc:LineExtensionAmount), '###.##0,00', 'european')"/>
                                    </xsl:if>
                                    <xsl:if test="$DocumentCurrency = 'TL'">
                                      <xsl:text>TL</xsl:text>
                                    </xsl:if>
                                    <xsl:if test="$DocumentCurrency != 'TL'">
                                      <xsl:value-of select="$DocumentCurrency"/>
                                    </xsl:if>
                                  </td>
                                </tr>
                                <tr>
                                  <td id="boldTd">&#160;Tevkifata Tabi İşlem Üzerinden Hes. KDV </td>
                                  <td style="text-align:right">
                                    <xsl:if test = "n1:Invoice/cac:InvoiceLine[cac:WithholdingTaxTotal/cac:TaxSubtotal/cac:TaxCategory/cac:TaxScheme]">
                                      <xsl:value-of
                                        select="format-number(sum(n1:Invoice/cac:WithholdingTaxTotal/cac:TaxSubtotal[cac:TaxCategory/cac:TaxScheme]/cbc:TaxableAmount), '###.##0,00', 'european')"/>
                                    </xsl:if>
                                    <xsl:if test = "//n1:Invoice/cac:TaxTotal/cac:TaxSubtotal/cac:TaxCategory/cac:TaxScheme/cbc:TaxTypeCode=&apos;9015&apos;">
                                      <xsl:value-of
                                        select="format-number(sum(n1:Invoice/cac:TaxTotal/cac:TaxSubtotal[cac:TaxCategory/cac:TaxScheme/cbc:TaxTypeCode=9015]/cbc:TaxableAmount), '###.##0,00', 'european')"/>
                                    </xsl:if>
                                    <xsl:if test="$DocumentCurrency = 'TL'">
                                      <xsl:text>TL</xsl:text>
                                    </xsl:if>
                                    <xsl:if test="$DocumentCurrency != 'TL'">
                                      <xsl:value-of select="$DocumentCurrency"/>
                                    </xsl:if>
                                  </td>
                                </tr>
                              </xsl:if>
                              <tr>
                                <td id="boldTd">&#160;Vergiler Dahil Toplam Tutar </td>
                                <td style="text-align:right">
                                  <xsl:for-each select="n1:Invoice/cac:LegalMonetaryTotal/cbc:TaxInclusiveAmount">
                                    <xsl:call-template name="Curr_Type"/>
                                  </xsl:for-each>
                                </td>
                              </tr>
                              <tr>
                                <td id="boldTd">&#160;Ödenecek Tutar </td>
                                <td style="text-align:right">
                                  <xsl:for-each select="n1:Invoice/cac:LegalMonetaryTotal/cbc:PayableAmount">
                                    <xsl:call-template name="Curr_Type"/>
                                  </xsl:for-each>
                                </td>
                              </tr>

                            </xsl:if>
                            <xsl:if test="$DocumentCurrency != 'TL' and //n1:Invoice/cac:PricingExchangeRate/cbc:CalculationRate">

                              <xsl:if test="$DocumentCurrency != 'TL' and //n1:Invoice/cac:PricingExchangeRate/cbc:CalculationRate"><tr><td id="boldTd">&#160;Mal Hizmet Toplam Tutarı(TL) </td><td style="text-align:right"><xsl:value-of select="format-number(//n1:Invoice/cac:LegalMonetaryTotal/cbc:LineExtensionAmount * //n1:Invoice/cac:PricingExchangeRate/cbc:CalculationRate, '###.##0,00', 'european')"/><xsl:text> TL</xsl:text></td></tr><tr><td id="boldTd">&#160;Toplam İskonto(TL)</td><td style="text-align:right"><xsl:value-of select="format-number(//n1:Invoice/cac:LegalMonetaryTotal/cbc:AllowanceTotalAmount * //n1:Invoice/cac:PricingExchangeRate/cbc:CalculationRate, '###.##0,00', 'european')"/><xsl:text> TL</xsl:text></td></tr><!--SUBTOTAL2--><xsl:for-each select="n1:Invoice/cac:TaxTotal/cac:TaxSubtotal"><!--MATRAH2--><tr><td id="boldTd">&#160;<xsl:text>Hesaplanan </xsl:text><xsl:value-of select="cac:TaxCategory/cac:TaxScheme/cbc:Name"/><xsl:text>(%</xsl:text><xsl:value-of select="cbc:Percent"/><xsl:text>) (TL)</xsl:text></td><td style="text-align:right"><xsl:value-of select="format-number(cbc:TaxAmount * //n1:Invoice/cac:PricingExchangeRate/cbc:CalculationRate, '###.##0,00', 'european')"/><xsl:text> TL</xsl:text></td></tr></xsl:for-each><tr><td id="boldTd">&#160;Vergiler Dahil Toplam Tutar(TL) </td><td style="text-align:right"><xsl:value-of select="format-number(//n1:Invoice/cac:LegalMonetaryTotal/cbc:TaxInclusiveAmount * //n1:Invoice/cac:PricingExchangeRate/cbc:CalculationRate, '###.##0,00', 'european')"/><xsl:text> TL</xsl:text></td></tr><tr><td id="boldTd">&#160;Ödenecek Tutar(TL) </td><td style="text-align:right"><xsl:value-of select="format-number(//n1:Invoice/cac:LegalMonetaryTotal/cbc:PayableAmount * //n1:Invoice/cac:PricingExchangeRate/cbc:CalculationRate, '###.##0,00', 'european')"/><xsl:text> TL</xsl:text></td></tr></xsl:if>
                            </xsl:if>
                          </tbody>
                        </table>
                      </td>
                    </tr>
                  </table>

                  <!--dip toplam-->
                </td>
              </tr>
              <tr>
                <td>
                  <!--iade bilgisi-->
                  <xsl:variable name="iade">
                    <xsl:value-of select="//n1:Invoice/cac:BillingReference/cac:InvoiceDocumentReference"/>
                  </xsl:variable>
                  <xsl:if test="contains($iade,'İADE') or contains($iade,'IADE') or contains($iade,'İade') or contains($iade,'Iade') or contains($iade,'iade')">
                    <br/>
                    <table id="lineTable" width="800">
                      <thead>
                        <tr>
                          <td colspan ="2" align="left" id="boldTd">
                            &#160;&#160;&#160;&#160;&#160;İadeye Konu Olan Faturalar
                          </td>
                        </tr>
                      </thead>
                      <tbody>
                        <tr align="left" class="lineTableTr">
                          <td id="boldTd">
                            &#160;&#160;&#160;&#160;&#160;Fatura No
                          </td>
                          <td id="boldTd">
                            &#160;&#160;&#160;&#160;&#160;Tarih
                          </td>
                        </tr>
                        <xsl:for-each select="//n1:Invoice/cac:BillingReference/cac:InvoiceDocumentReference">
                          <tr align="left" class="lineTableTr">
                            <td>
                              &#160;&#160;&#160;&#160;&#160;
                              <xsl:value-of select="./cbc:ID"/>
                            </td>
                            <td>
                              &#160;&#160;&#160;&#160;&#160;
                              <xsl:for-each select="./cbc:IssueDate">
                                <xsl:apply-templates select="."/>
                              </xsl:for-each>
                            </td>
                          </tr>
                        </xsl:for-each>
                      </tbody>
                    </table>
                  </xsl:if>
                  <!--iade bilgisi-->
                </td>
              </tr>
              <tr>
                <td>
                  <!--ökc bilgisi-->
                  <xsl:if	test="//n1:Invoice/cac:BillingReference/cac:AdditionalDocumentReference/cbc:DocumentTypeCode='OKCBF'">
                    <br/>
                    <table id="lineTable" width="800">
                      <thead>
                        <tr>
                          <th colspan="6">ÖKC Bilgileri</th>
                        </tr>
                      </thead>
                      <tbody>
                        <tr id="boldTd">
                          <td style="width:20%">
                            <xsl:text>Fiş Numarası</xsl:text>
                          </td>
                          <td style="width:10%" align="center">
                            <xsl:text>Fiş Tarihi</xsl:text>
                          </td>
                          <td style="width:10%" align="center">
                            <xsl:text>Fiş Saati</xsl:text>
                          </td>
                          <td style="width:40%" align="center">
                            <xsl:text>Fiş Tipi</xsl:text>
                          </td>
                          <td style="width:10%" align="center">
                            <xsl:text>Z Rapor No</xsl:text>
                          </td>
                          <td style="width:10%" align="center">
                            <xsl:text>ÖKC Seri No</xsl:text>
                          </td>
                        </tr>
                      </tbody>
                      <xsl:for-each select="//n1:Invoice/cac:BillingReference/cac:AdditionalDocumentReference/cbc:DocumentTypeCode[text()='OKCBF']">
                        <tr>
                          <td style="width:20%">
                            <xsl:value-of select="../cbc:ID"/>
                          </td>
                          <td style="width:10%" align="center">
                            <xsl:value-of select="../cbc:IssueDate"/>
                          </td>
                          <td style="width:10%" align="center">
                            <xsl:value-of select="substring(../cac:ValidityPeriod/cbc:StartTime,1,5)"/>
                          </td>
                          <td style="width:40%" align="center">
                            <xsl:choose>
                              <xsl:when test="../cbc:DocumentDescription='AVANS'">
                                <xsl:text>Ön Tahsilat(Avans) Bilgi Fişi</xsl:text>
                              </xsl:when>
                              <xsl:when test="../cbc:DocumentDescription='YEMEK_FIS'">
                                <xsl:text>Yemek Fişi/Kartı ile Yapılan Tahsilat Bilgi Fişi</xsl:text>
                              </xsl:when>
                              <xsl:when test="../cbc:DocumentDescription='E-FATURA'">
                                <xsl:text>E-Fatura Bilgi Fişi</xsl:text>
                              </xsl:when>
                              <xsl:when test="../cbc:DocumentDescription='E-FATURA_IRSALIYE'">
                                <xsl:text>İrsaliye Yerine Geçen E-Fatura Bilgi Fişi</xsl:text>
                              </xsl:when>
                              <xsl:when test="../cbc:DocumentDescription='E-ARSIV'">
                                <xsl:text>E-Arşiv Bilgi Fişi</xsl:text>
                              </xsl:when>
                              <xsl:when test="../cbc:DocumentDescription='E-ARSIV_IRSALIYE'">
                                <xsl:text>İrsaliye Yerine Geçen E-Arşiv Bilgi Fişi</xsl:text>
                              </xsl:when>
                              <xsl:when test="../cbc:DocumentDescription='FATURA'">
                                <xsl:text>Faturalı Satış Bilgi Fişi</xsl:text>
                              </xsl:when>
                              <xsl:when test="../cbc:DocumentDescription='OTOPARK'">
                                <xsl:text>Otopark Giriş Bilgi Fişi</xsl:text>
                              </xsl:when>
                              <xsl:when test="../cbc:DocumentDescription='FATURA_TAHSILAT'">
                                <xsl:text>Fatura Tahsilat Bilgi Fişi</xsl:text>
                              </xsl:when>
                              <xsl:when test="../cbc:DocumentDescription='FATURA_TAHSILAT_KOMISYONLU'">
                                <xsl:text>Komisyonlu Fatura Tahsilat Bilgi Fişi</xsl:text>
                              </xsl:when>
                              <xsl:otherwise>
                                <xsl:text> </xsl:text>
                              </xsl:otherwise>
                            </xsl:choose>
                          </td>
                          <td style="width:10%" align="center">
                            <xsl:value-of select="../cac:Attachment/cac:ExternalReference/cbc:URI"/>
                          </td>
                          <td style="width:10%" align="center">
                            <xsl:value-of select="../cac:IssuerParty/cbc:EndpointID"/>
                          </td>
                        </tr>
                      </xsl:for-each>
                    </table>
                  </xsl:if>
                  <!--ökc bilgisi-->
                </td>
              </tr>
              <tr>
                <td>
                  <!--notlar-->
                  <table id ="lineTable" width="800">
                    <tbody>
                      <tr align="left">
                        <td height="100">
                          <xsl:for-each select="//n1:Invoice/cac:TaxTotal/cac:TaxSubtotal">
                            <xsl:if	test="(cac:TaxCategory/cac:TaxScheme/cbc:TaxTypeCode='0015' or ../../cbc:InvoiceTypeCode='OZELMATRAH') and cac:TaxCategory/cbc:TaxExemptionReason">
                              <b>&#160;&#160;&#160;&#160;&#160; Vergi İstisna Muafiyet Sebebi: </b>
                              <xsl:value-of select="cac:TaxCategory/cbc:TaxExemptionReasonCode"/>
                              <xsl:text>-</xsl:text>
                              <xsl:value-of select="cac:TaxCategory/cbc:TaxExemptionReason"/>
                              <br/>
                            </xsl:if>
                            <xsl:if	test="starts-with(cac:TaxCategory/cac:TaxScheme/cbc:TaxTypeCode,'007') and cac:TaxCategory/cbc:TaxExemptionReason">
                              <b>&#160;&#160;&#160;&#160;&#160; ÖTV İstisna Muafiyet Sebebi: </b>
                              <xsl:value-of select="cac:TaxCategory/cbc:TaxExemptionReasonCode"/>
                              <xsl:text>-</xsl:text>
                              <xsl:value-of select="cac:TaxCategory/cbc:TaxExemptionReason"/>
                              <br/>
                            </xsl:if>
                          </xsl:for-each>
                          <xsl:for-each select="//n1:Invoice/cac:WithholdingTaxTotal/cac:TaxSubtotal/cac:TaxCategory/cac:TaxScheme">
                            <b>&#160;&#160;&#160;&#160;&#160; Tevkifat Sebebi: </b>
                            <xsl:value-of select="cbc:TaxTypeCode"/>
                            <xsl:text>-</xsl:text>
                            <xsl:value-of select="cbc:Name"/>
                            <br/>
                          </xsl:for-each>
                          <xsl:for-each select="//n1:Invoice/cbc:Note">
                            <b>&#160;&#160;&#160;&#160;&#160; Not: </b>
                            <xsl:value-of select="." disable-output-escaping="yes"/>
                            <br/>
                          </xsl:for-each>
                          <xsl:if test="//n1:Invoice/cac:PaymentMeans/cbc:InstructionNote">
                            <b>&#160;&#160;&#160;&#160;&#160; Ödeme Notu: </b>
                            <xsl:value-of
                              select="//n1:Invoice/cac:PaymentMeans/cbc:InstructionNote"/>
                            <br/>
                          </xsl:if>
                          <xsl:if
                            test="//n1:Invoice/cac:PaymentMeans/cac:PayeeFinancialAccount/cbc:PaymentNote">
                            <b>&#160;&#160;&#160;&#160;&#160; Hesap Açıklaması: </b>
                            <xsl:value-of
                              select="//n1:Invoice/cac:PaymentMeans/cac:PayeeFinancialAccount/cbc:PaymentNote"/>
                            <br/>
                          </xsl:if>
                          <xsl:if test="//n1:Invoice/cac:PaymentTerms/cbc:Note">
                            <b>&#160;&#160;&#160;&#160;&#160; Ödeme Koşulu: </b>
                            <xsl:value-of select="//n1:Invoice/cac:PaymentTerms/cbc:Note"/>
                            <br/>
                          </xsl:if>

                        </td>
                      </tr>
                    </tbody>
                  </table>
                  <!--notlar-->
                  <br/>
                  <br/>
                  <div id="frmBankAccount"></div>
                  <br/>
                  <div id="frmOtherInfo"></div>
                  <!--BANKA-->
                </td>
              </tr>
              <tr>
                <td>


                  <xsl:for-each select="//n1:Invoice/cac:AdditionalDocumentReference">
                    <xsl:if test="./cbc:ID/@schemeID = 'XSLTDISPATCH'">
                      <br />
                      <table width="800" border="1" height="50">
                        <tr>
                          <td>
                            <span style="margin-left:10px">
                              <xsl:value-of select="./cbc:ID"/>
                            </span>
                          </td>
                        </tr>
                      </table>
                    </xsl:if>
                  </xsl:for-each>
                </td>
              </tr>
              <tr>
                <td>

                  <xsl:for-each select="//n1:Invoice/cac:AdditionalDocumentReference">
                    <xsl:if test="./cbc:ID/@schemeID = 'XSLTINTERNET'">
                      <br />
                      <table width="800" border="1" height="50">
                        <tr>
                          <td>
                            <span style="margin-left:10px">
                              <xsl:value-of select="./cbc:ID"/>
                            </span>
                          </td>
                        </tr>
                      </table>
                      <br />
                      <table border="1" width="800" height="50" id="info">
                        <tr>
                          <td style="font-weight: bold;">
                            Web Adresi
                          </td>
                          <td>

                            <xsl:for-each select="//n1:Invoice/cac:AdditionalDocumentReference">
                              <xsl:if test="./cbc:ID/@schemeID = 'INTWEBADRES'">
                                <xsl:value-of select="./cbc:ID"/>
                              </xsl:if>
                            </xsl:for-each>
                          </td>
                          <td style="font-weight: bold;">
                            Ödeme Şekli
                          </td>
                          <td>
                            <xsl:for-each select="//n1:Invoice/cac:AdditionalDocumentReference">
                              <xsl:if test="./cbc:ID/@schemeID = 'INTODEMESEKLI'">
                                <xsl:value-of select="./cbc:ID"/>
                              </xsl:if>
                            </xsl:for-each>
                          </td>
                          <td style="font-weight: bold;">
                            Ödeme Tarihi
                          </td>
                          <td>
                            <xsl:for-each select="//n1:Invoice/cac:AdditionalDocumentReference">
                              <xsl:if test="./cbc:ID/@schemeID = 'INTODEMETARIHI'">
                                <xsl:value-of select="./cbc:ID"/>
                              </xsl:if>
                            </xsl:for-each>
                          </td>

                        </tr>
                        <tr>
                          <td style="font-weight: bold;">
                            Gönderiyi Taşıyan
                          </td>
                          <td>
                            <xsl:for-each select="//n1:Invoice/cac:AdditionalDocumentReference">
                              <xsl:if test="./cbc:ID/@schemeID = 'INTGONDERITASIYAN'">
                                <xsl:value-of select="./cbc:ID"/>
                              </xsl:if>
                            </xsl:for-each>
                          </td>
                          <td style="font-weight: bold;">
                            Taşıyıcı VKN
                          </td>
                          <td>
                            <xsl:for-each select="//n1:Invoice/cac:AdditionalDocumentReference">
                              <xsl:if test="./cbc:ID/@schemeID = 'INTTASIYICIVKN'">
                                <xsl:value-of select="./cbc:ID"/>
                              </xsl:if>
                            </xsl:for-each>
                          </td>
                          <td style="font-weight: bold;">
                            Gönderim Tarihi
                          </td>
                          <td>
                            <xsl:for-each select="//n1:Invoice/cac:AdditionalDocumentReference">
                              <xsl:if test="./cbc:ID/@schemeID = 'INTGONDERIMTARIHI'">
                                <xsl:value-of select="./cbc:ID"/>
                              </xsl:if>
                            </xsl:for-each>
                          </td>

                        </tr>
                      </table>

                      <br />
                      <table id="info" border="1" height="50" style="margin-bottom:10px" width="800">
                        <tbody>
                          <tr style="text-align:center">
                            <td colspan="5" style="font-weight:bold">İADE BÖLÜMÜ</td>
                          </tr>
                          <tr style="border:0px">
                            <td style="width:150px; text-align:center">Stok Kodu</td>
                            <td style="width:350px; text-align:center">Cinsi</td>
                            <td style="width:100px; text-align:center">Adet</td>
                            <td style="width:100px; text-align:center">Birim Fiyatı</td>
                            <td style="width:100px; text-align:center">Tutarı</td>
                          </tr>
                          <tr>
                            <td style="height:18px; border-bottom:0px"></td>
                            <td style="border-bottom: 0px none;"></td>
                            <td style="border-bottom: 0px none;"></td>
                            <td style="border-bottom: 0px none;"></td>
                            <td style="border-bottom: 0px none;"></td>
                          </tr>
                          <tr>
                            <td style="height:18px; border-top:0px; border-bottom:0px"></td>
                            <td style="border-top: 0px none; border-bottom: 0px none;"></td>
                            <td style="border-top: 0px none; border-bottom: 0px none;"></td>
                            <td style="border-top: 0px none; border-bottom: 0px none;"></td>
                            <td style="border-top: 0px none; border-bottom: 0px none;"></td>
                          </tr>
                          <tr>
                            <td style="height:18px; border-top:0px"></td>
                            <td style="border-top: 0px none;"></td>
                            <td style="border-top: 0px none;"></td>
                            <td style="border-top: 0px none;"></td>
                            <td style="border-top: 0px none;"></td>
                          </tr>
                        </tbody>
                      </table>
                      <span>İade Edenin Adı, Soyadı:</span>
                      <span style="margin-left:250px">İmzası:</span>
                      <br/>
                      <span>Adresi: </span>
                      <br />
                      <br />

                    </xsl:if>
                  </xsl:for-each>
                </td>
              </tr>
              <tr>
                <td>

                  <xsl:for-each select="//n1:Invoice/cac:AdditionalDocumentReference">
                    <xsl:if test="./cbc:ID/@schemeID = 'XSLTELECTRONIC'">

                      <br />
                      <table width="800" border="1" height="50">
                        <tr>
                          <td>
                            <span style="margin-left:10px">
                              <xsl:value-of select="./cbc:ID"/>
                            </span>
                          </td>

                        </tr>

                      </table>

                    </xsl:if>
                  </xsl:for-each>
                </td>
              </tr>
            </table>
          </td>
        </tr>


      </body>
    </html>
  </xsl:template>
  <!--burası satır - kalem içerikilerinin geldiği yer-->
  <xsl:template match="//n1:Invoice/cac:InvoiceLine">
    <tr>
      <!--sırano-->
      <td>
        <xsl:text>&#160;</xsl:text>
        <xsl:value-of select="./cbc:ID"/>
      </td>
      <!--stok kodu-->
      <!--SELLERID2-->
      <!--mal hizmet-->
      <td>
        <xsl:text>&#160;</xsl:text>
        <xsl:value-of select="./cac:Item/cbc:Name"/>
      </td>
      <!--üretici kodu-->
      <!--MANUFACTURERID2-->
      <!--alıcı kodu-->
      <!--BUYERID2-->
      <!--açıklama-->
      <!--DESCRIPTION2-->
      <!--satır notu-->
      <!--LINENOTE2-->
      <!--miktar-->
      <td align="right">
        <xsl:for-each select="cbc:InvoicedQuantity">
          <xsl:call-template name="Atr_Type"/>
        </xsl:for-each>
      </td>
      <!--birim fiyat-->
      <td align="right">
        <xsl:text>&#160;</xsl:text>
        <xsl:value-of
					select="format-number(./cac:Price/cbc:PriceAmount, '###.##0,00####', 'european')"/>
        <xsl:if test="./cac:Price/cbc:PriceAmount/@currencyID">
          <xsl:text> </xsl:text>
          <xsl:if test="./cac:Price/cbc:PriceAmount/@currencyID = &quot;TRL&quot; or ./cac:Price/cbc:PriceAmount/@currencyID = &quot;TRY&quot;">
            <xsl:text>TL</xsl:text>
          </xsl:if>
          <xsl:if test="./cac:Price/cbc:PriceAmount/@currencyID != &quot;TRL&quot; and ./cac:Price/cbc:PriceAmount/@currencyID != &quot;TRY&quot;">
            <xsl:value-of select="./cac:Price/cbc:PriceAmount/@currencyID"/>
          </xsl:if>
        </xsl:if>
      </td>
	  <!--iskonto orannı-->
      <td align="right"> <xsl:text>&#160;</xsl:text> <xsl:for-each select="./cac:AllowanceCharge/cbc:MultiplierFactorNumeric"> <xsl:text> %</xsl:text> <xsl:value-of select="format-number(. * 100, '###.##0,00', 'european')"/> <br/> </xsl:for-each> </td>
      <!--iskonto tutarı-->
      <td align="right"><xsl:text>&#160;</xsl:text><xsl:for-each select="cac:AllowanceCharge/cbc:Amount"><xsl:call-template name="Curr_Type"/><br/></xsl:for-each></td>
      <!--kdv oranı-->
      <td align="right"><xsl:text>&#160;</xsl:text><xsl:for-each select="./cac:TaxTotal/cac:TaxSubtotal/cac:TaxCategory/cac:TaxScheme"><xsl:if test="cbc:TaxTypeCode='0015' "><xsl:text> </xsl:text><xsl:if test="../../cbc:Percent"><xsl:text> %</xsl:text><xsl:value-of select="format-number(../../cbc:Percent, '###.##0,00', 'european')"/></xsl:if></xsl:if></xsl:for-each></td>
      <!--kdv  tutarı-->
      <td align="right"><xsl:text>&#160;</xsl:text><xsl:for-each select="./cac:TaxTotal/cac:TaxSubtotal/cac:TaxCategory/cac:TaxScheme"><xsl:if test="cbc:TaxTypeCode='0015' "><xsl:text> </xsl:text><xsl:for-each select="../../cbc:TaxAmount"><xsl:call-template name="Curr_Type"/></xsl:for-each></xsl:if></xsl:for-each></td>
      <!--diğer vergiler-->
      <td style="font-size: xx-small" align="right"><xsl:text>&#160;</xsl:text><xsl:for-each select="./cac:TaxTotal/cac:TaxSubtotal/cac:TaxCategory/cac:TaxScheme"><xsl:if test="cbc:TaxTypeCode!='0015' "><xsl:text> </xsl:text><xsl:value-of select="cbc:Name"/><xsl:if test="../../cbc:Percent"><xsl:text> (%</xsl:text><xsl:value-of select="format-number(../../cbc:Percent, '###.##0,00', 'european')"/><xsl:text>)=</xsl:text></xsl:if><xsl:for-each select="../../cbc:TaxAmount"><xsl:call-template name="Curr_Type"/></xsl:for-each></xsl:if></xsl:for-each><xsl:for-each select="./cac:WithholdingTaxTotal/cac:TaxSubtotal/cac:TaxCategory/cac:TaxScheme"><xsl:text>KDV TEVKİFAT </xsl:text><xsl:if test="../../cbc:Percent"><xsl:text> (%</xsl:text><xsl:value-of select="format-number(../../cbc:Percent, '###.##0,00', 'european')"/><xsl:text>)=</xsl:text></xsl:if><xsl:for-each select="../../cbc:TaxAmount"><xsl:call-template name="Curr_Type"/><xsl:text>&#10;</xsl:text></xsl:for-each></xsl:for-each><xsl:text>&#160;</xsl:text><xsl:value-of select="./cac:Item/cbc:Note"/></td>
      <!--mal hizmet tutarı-->
      <td align="right">
        <xsl:text>&#160;</xsl:text>
        <xsl:for-each select="cbc:LineExtensionAmount">
          <xsl:call-template name="Curr_Type"/>
        </xsl:for-each>
      </td>
    </tr>
  </xsl:template>
  <!--tarih formatı-->
  <xsl:template match="//cbc:IssueDate">
    <xsl:value-of select="substring(.,9,2)"/>-<xsl:value-of select="substring(.,6,2)"/>-<xsl:value-of select="substring(.,1,4)"/>
  </xsl:template>
  <!--para birimi-->
  <xsl:template name="Curr_Type">
    <xsl:value-of select="format-number(., '###.##0,00', 'european')"/>
    <xsl:if	test="@currencyID">
      <xsl:text> </xsl:text>
      <xsl:choose>
        <xsl:when test="@currencyID = 'TRL' or @currencyID = 'TRY'">
          <xsl:text>TL</xsl:text>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="@currencyID"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:if>
  </xsl:template>
  <!--birim karşılığı-->
  <xsl:template name="Atr_Type">
    <xsl:text>&#160;</xsl:text>
    <xsl:value-of select="format-number(., '###.##0,####', 'european')"/>
    <xsl:if test="@unitCode">
      <xsl:for-each select=".">
        <xsl:text> </xsl:text>
        <xsl:choose>
          <xsl:when test="@unitCode  = 'TNE'">
            <xsl:text>ton</xsl:text>
          </xsl:when>
          <xsl:when test="@unitCode  = 'BX'">
            <xsl:text>Kutu</xsl:text>
          </xsl:when>
          <xsl:when test="@unitCode  = 'PK'">
            <xsl:text>Paket</xsl:text>
          </xsl:when>
          <xsl:when test="@unitCode  = 'LTR'">
            <xsl:text>lt</xsl:text>
          </xsl:when>
          <xsl:when test="@unitCode  = 'C62'">
            <xsl:text>Adet</xsl:text>
          </xsl:when>
          <xsl:when test="@unitCode  = 'NIU'">
            <xsl:text>Adet</xsl:text>
          </xsl:when>
          <xsl:when test="@unitCode  = 'KGM'">
            <xsl:text>kg</xsl:text>
          </xsl:when>
          <xsl:when test="@unitCode  = 'KJO'">
            <xsl:text>kJ</xsl:text>
          </xsl:when>
          <xsl:when test="@unitCode  = 'GRM'">
            <xsl:text>g</xsl:text>
          </xsl:when>
          <xsl:when test="@unitCode  = 'MGM'">
            <xsl:text>mg</xsl:text>
          </xsl:when>
          <xsl:when test="@unitCode  = 'NT'">
            <xsl:text>Net Ton</xsl:text>
          </xsl:when>
          <xsl:when test="@unitCode  = 'GT'">
            <xsl:text>Gross Ton</xsl:text>
          </xsl:when>
          <xsl:when test="@unitCode  = 'MTR'">
            <xsl:text>m</xsl:text>
          </xsl:when>
          <xsl:when test="@unitCode  = 'MMT'">
            <xsl:text>mm</xsl:text>
          </xsl:when>
          <xsl:when test="@unitCode  = 'KTM'">
            <xsl:text>km</xsl:text>
          </xsl:when>
          <xsl:when test="@unitCode  = 'MLT'">
            <xsl:text>ml</xsl:text>
          </xsl:when>
          <xsl:when test="@unitCode  = 'MMQ'">
            <xsl:text>mm3</xsl:text>
          </xsl:when>
          <xsl:when test="@unitCode  = 'CLT'">
            <xsl:text>cl</xsl:text>
          </xsl:when>
          <xsl:when test="@unitCode  = 'CMK'">
            <xsl:text>cm2</xsl:text>
          </xsl:when>
          <xsl:when test="@unitCode  = 'CMQ'">
            <xsl:text>cm3</xsl:text>
          </xsl:when>
          <xsl:when test="@unitCode  = 'CMT'">
            <xsl:text>cm</xsl:text>
          </xsl:when>
          <xsl:when test="@unitCode  = 'MTK'">
            <xsl:text>m2</xsl:text>
          </xsl:when>
          <xsl:when test="@unitCode  = 'MTQ'">
            <xsl:text>m3</xsl:text>
          </xsl:when>
          <xsl:when test="@unitCode  = 'DAY'">
            <xsl:text> Gün</xsl:text>
          </xsl:when>
          <xsl:when test="@unitCode  = 'MON'">
            <xsl:text> Ay</xsl:text>
          </xsl:when>
          <xsl:when test="@unitCode  = 'PA'">
            <xsl:text> Paket</xsl:text>
          </xsl:when>
          <xsl:when test="@unitCode  = 'KWH'">
            <xsl:text> KWH</xsl:text>
          </xsl:when>
          <xsl:when test="@unitCode  = 'ANN'">
            <xsl:text> Yıl</xsl:text>
          </xsl:when>
          <xsl:when test="@unitCode  = 'HUR'">
            <xsl:text> Saat</xsl:text>
          </xsl:when>
          <xsl:when test="@unitCode  = 'D61'">
            <xsl:text> Dakika</xsl:text>
          </xsl:when>
          <xsl:when test="@unitCode  = 'D62'">
            <xsl:text> Saniye</xsl:text>
          </xsl:when>
          <xsl:when test="@unitCode  = 'CCT'">
            <xsl:text> Ton baş.taşıma kap.</xsl:text>
          </xsl:when>
          <xsl:when test="@unitCode  = 'D30'">
            <xsl:text> Brüt kalori</xsl:text>
          </xsl:when>
          <xsl:when test="@unitCode  = 'D40'">
            <xsl:text> 1000 lt</xsl:text>
          </xsl:when>
          <xsl:when test="@unitCode  = 'LPA'">
            <xsl:text> saf alkol lt</xsl:text>
          </xsl:when>
          <xsl:when test="@unitCode  = 'B32'">
            <xsl:text> kg.m2</xsl:text>
          </xsl:when>
          <xsl:when test="@unitCode  = 'NCL'">
            <xsl:text> hücre adet</xsl:text>
          </xsl:when>
          <xsl:when test="@unitCode  = 'PR'">
            <xsl:text> Çift</xsl:text>
          </xsl:when>
          <xsl:when test="@unitCode  = 'R9'">
            <xsl:text> 1000 m3</xsl:text>
          </xsl:when>
          <xsl:when test="@unitCode  = 'SET'">
            <xsl:text> Set</xsl:text>
          </xsl:when>
          <xsl:when test="@unitCode  = 'T3'">
            <xsl:text> 1000 adet</xsl:text>
          </xsl:when>
        </xsl:choose>
      </xsl:for-each>
    </xsl:if>
  </xsl:template>
  <xsl:character-map name="a">
    <xsl:output-character character="&#128;" string=""/>
    <xsl:output-character character="&#129;" string=""/>
    <xsl:output-character character="&#130;" string=""/>
    <xsl:output-character character="&#131;" string=""/>
    <xsl:output-character character="&#132;" string=""/>
    <xsl:output-character character="&#133;" string=""/>
    <xsl:output-character character="&#134;" string=""/>
    <xsl:output-character character="&#135;" string=""/>
    <xsl:output-character character="&#136;" string=""/>
    <xsl:output-character character="&#137;" string=""/>
    <xsl:output-character character="&#138;" string=""/>
    <xsl:output-character character="&#139;" string=""/>
    <xsl:output-character character="&#140;" string=""/>
    <xsl:output-character character="&#141;" string=""/>
    <xsl:output-character character="&#142;" string=""/>
    <xsl:output-character character="&#143;" string=""/>
    <xsl:output-character character="&#144;" string=""/>
    <xsl:output-character character="&#145;" string=""/>
    <xsl:output-character character="&#146;" string=""/>
    <xsl:output-character character="&#147;" string=""/>
    <xsl:output-character character="&#148;" string=""/>
    <xsl:output-character character="&#149;" string=""/>
    <xsl:output-character character="&#150;" string=""/>
    <xsl:output-character character="&#151;" string=""/>
    <xsl:output-character character="&#152;" string=""/>
    <xsl:output-character character="&#153;" string=""/>
    <xsl:output-character character="&#154;" string=""/>
    <xsl:output-character character="&#155;" string=""/>
    <xsl:output-character character="&#156;" string=""/>
    <xsl:output-character character="&#157;" string=""/>
    <xsl:output-character character="&#158;" string=""/>
    <xsl:output-character character="&#159;" string=""/>
  </xsl:character-map>
  <xsl:template name ="FirmaBilgisi">
    <xsl:for-each select="./cac:Party">
      <table>
        <thead>
          <tr>
            <td width ="100">
            </td>
            <td width ="200">
            </td>
          </tr>
        </thead>
        <tbody>
          <tr>
            <td colspan ="2">
              <b>
                <xsl:value-of select="cac:PartyName/cbc:Name"/>
              </b>
            </td>
          </tr>
          <xsl:if test="./cac:Person">
            <tr>
              <td colspan ="2">
                <xsl:value-of select="./cac:Person/cbc:FirstName"/>
                <xsl:text>&#160;</xsl:text>
                <xsl:value-of select="./cac:Person/cbc:FamilyName"/>
                <xsl:text>&#160;</xsl:text>
                <xsl:value-of select="./cac:Person/cbc:MiddleName"/>
                <xsl:text>&#160;</xsl:text>
                <xsl:value-of select="./cac:Person/cbc:Title"/>
                <xsl:text>&#160;</xsl:text>
                <xsl:value-of select="./cac:Person/cbc:NameSuffix"/>
              </td>
            </tr>
            <xsl:if test="./cac:Party/cac:PartyIdentification/cbc:ID ='TAXFREE'">
              <tr>
                <td>
                  <xsl:text>Pasaport No </xsl:text>
                </td>
                <td>
                  :<xsl:value-of select="cac:IdentityDocumentReference/cbc:ID"/>
                </td>
              </tr>
              <tr>
                <td>
                  <xsl:text>Ülkesi </xsl:text>
                </td>
                <td>
                  :<xsl:for-each select="cbc:NationalityID">
                    <xsl:variable name="NationalityID" select="."/>
                    <xsl:value-of select="."/>
                  </xsl:for-each>
                </td>
              </tr>
            </xsl:if>
          </xsl:if>
          <xsl:if test ="./cac:PostalAddress">
            <xsl:if test ="./cac:PostalAddress/cbc:StreetName[text()!=''] or ./cac:PostalAddress/cbc:BuildingName[text()!=''] or ./cac:PostalAddress/cbc:Room[text()!=''] or ./cac:PostalAddress/cbc:PostalZone[text()!='']">
              <tr>
                <td colspan ="2">
                  <xsl:value-of select="./cac:PostalAddress/cbc:StreetName"/>
                  <xsl:text>&#160;</xsl:text>
                  <xsl:value-of select="./cac:PostalAddress/cbc:BuildingName"/>
                  <xsl:text>&#160;</xsl:text>
                  <xsl:value-of select="./cac:PostalAddress/cbc:BuildingNumber"/>
                  <xsl:text>&#160;</xsl:text>
                  <xsl:value-of select="./cac:PostalAddress/cbc:Room"/>
				  <xsl:text>&#160;</xsl:text>
				   <xsl:value-of select="./cac:PostalAddress/cbc:District"/>
                  <xsl:text>&#160;</xsl:text>
                  <xsl:value-of select="./cac:PostalAddress/cbc:PostalZone"/>
                </td>
              </tr>
            </xsl:if>
            <tr>
              <td colspan ="2">
                <xsl:value-of select="./cac:PostalAddress/cbc:CitySubdivisionName"/>
                <xsl:text>&#160;</xsl:text>
                <xsl:value-of select="./cac:PostalAddress/cbc:CityName"/>
                <xsl:if test="./cac:PartyIdentification/cbc:ID ='EXPORT' or ./cac:PartyIdentification/cbc:ID ='TAXFREE'">
                  <xsl:text>&#160;</xsl:text>
                  <xsl:value-of select="./cac:PostalAddress/cac:Country/cbc:Name"/>
                  <br/>
                </xsl:if>
              </td>
            </tr>
          </xsl:if>
          <xsl:if test ="./cbc:WebsiteURI[text()!='']">
            <tr>
              <td>
                <xsl:text>Web Sitesi </xsl:text>
              </td>
              <td>
                : <xsl:value-of select="cbc:WebsiteURI"/>
              </td>
            </tr>
          </xsl:if>
          <xsl:if test ="./cac:Contact/cbc:ElectronicMail[text()!='']">
            <tr>
              <td>
                <xsl:text>E-Posta </xsl:text>
              </td>
              <td>
                : <xsl:value-of select="cac:Contact/cbc:ElectronicMail"/>
              </td>
            </tr>
          </xsl:if>
          <xsl:if test ="./cac:Contact/cbc:Telephone[text()!='']">
            <tr>
              <td>
                <xsl:text>Tel </xsl:text>
              </td>
              <td>
                : <xsl:value-of select="cac:Contact/cbc:Telephone"/>
              </td>
            </tr>
          </xsl:if>
          <xsl:if test ="./cac:Contact/cbc:Telefax[text()!='']">
            <tr>
              <td>
                <xsl:text>Fax </xsl:text>
              </td>
              <td>
                : <xsl:value-of select="cac:Contact/cbc:Telefax"/>
              </td>
            </tr>
          </xsl:if>
          <xsl:variable name="PartyIdentification" select="./cac:PartyIdentification/cbc:ID"/>
          <xsl:if test="$PartyIdentification !='TAXFREE' and $PartyIdentification !='EXPORT'">
            <xsl:if test="cac:PartyTaxScheme/cac:TaxScheme/cbc:Name">
              <tr>
                <td>
                  <xsl:text>Vergi Dairesi </xsl:text>
                </td>
                <td>
                  : <xsl:value-of select ="cac:PartyTaxScheme/cac:TaxScheme/cbc:Name"/>
                </td>
              </tr>
            </xsl:if>
            <xsl:for-each select ="$PartyIdentification">
              <tr>
                <td>
                  <xsl:value-of select="./@schemeID"/>
                </td>
                <td>
                  <xsl:text>: </xsl:text>
                  <xsl:value-of select="."/>
                </td>
              </tr>
            </xsl:for-each>
          </xsl:if>
        </tbody>
      </table>
    </xsl:for-each>
  </xsl:template>
</xsl:stylesheet>


