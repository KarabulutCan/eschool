<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="2.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:cac="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2" xmlns:cbc="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2" xmlns:ccts="urn:un:unece:uncefact:documentation:2" xmlns:clm54217="urn:un:unece:uncefact:codelist:specification:54217:2001"	xmlns:clm5639="urn:un:unece:uncefact:codelist:specification:5639:1988"	xmlns:clm66411="urn:un:unece:uncefact:codelist:specification:66411:2001" xmlns:fn="http://www.w3.org/2005/xpath-functions" xmlns:link="http://www.xbrl.org/2003/linkbase"	xmlns:n1="urn:oasis:names:specification:ubl:schema:xsd:Invoice-2"	xmlns:qdt="urn:oasis:names:specification:ubl:schema:xsd:QualifiedDatatypes-2"	xmlns:udt="urn:un:unece:uncefact:data:specification:UnqualifiedDataTypesSchemaModule:2" xmlns:xbrldi="http://xbrl.org/2006/xbrldi" xmlns:xbrli="http://www.xbrl.org/2003/instance"	xmlns:xdt="http://www.w3.org/2005/xpath-datatypes" xmlns:xlink="http://www.w3.org/1999/xlink"	xmlns:xs="http://www.w3.org/2001/XMLSchema" xmlns:xsd="http://www.w3.org/2001/XMLSchema"	xmlns:lcl="http://www.efatura.gov.tr/local" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"	exclude-result-prefixes="cac cbc ccts clm54217 clm5639 clm66411  fn link n1 qdt udt xbrldi xbrli xdt xlink xs xsd xsi lcl">
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
        <title>e-Fatura</title>
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
                          <img style="width:91px;" align="middle" alt="E-Fatura Logo"
                            src="data:image/jpeg;base64,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" />
						  <h1 align="center">e-FATURA</h1>
                        </td>
                        <!--faturayı kesen firmanın logosu-->
                        <td width="40%" valign="top"  align="right">
							<div id="qrcode"/>
							<div id="qrvalue" style="visibility: hidden; height: 20px;width: 20px;display:none">{"vkntckn":"<xsl:value-of select="n1:Invoice/cac:AccountingSupplierParty/cac:Party/cac:PartyIdentification/cbc:ID[@schemeID = 'TCKN' or @schemeID = 'VKN']"/>","avkntckn":"<xsl:value-of select="n1:Invoice/cac:AccountingCustomerParty/cac:Party/cac:PartyIdentification/cbc:ID[@schemeID = 'TCKN' or @schemeID = 'VKN']"/><xsl:text></xsl:text>","senaryo":"<xsl:value-of select="n1:Invoice/cbc:ProfileID"/>","tip":"<xsl:value-of select="n1:Invoice/cbc:InvoiceTypeCode"/>","tarih":"<xsl:value-of select="n1:Invoice/cbc:IssueDate"/>","no":"<xsl:value-of select="n1:Invoice/cbc:ID"/>","ettn":"<xsl:value-of select="n1:Invoice/cbc:UUID"/>","parabirimi":"<xsl:value-of select="n1:Invoice/cbc:DocumentCurrencyCode"/>","malhizmettoplam":"<xsl:value-of select="n1:Invoice/cac:LegalMonetaryTotal/cbc:LineExtensionAmount"/><xsl:for-each select="n1:Invoice/cac:TaxTotal/cac:TaxSubtotal[cac:TaxCategory/cac:TaxScheme/cbc:TaxTypeCode = '0015']">"<xsl:text>,"kdvmatrah</xsl:text>(<xsl:value-of select="cbc:Percent"/>)":"<xsl:value-of select="cbc:TaxableAmount"/>"</xsl:for-each><xsl:for-each select="n1:Invoice/cac:TaxTotal/cac:TaxSubtotal[cac:TaxCategory/cac:TaxScheme/cbc:TaxTypeCode = '0015']"><xsl:text>,"hesaplanankdv</xsl:text>(<xsl:value-of select="cbc:Percent"/>)":"<xsl:value-of select="cbc:TaxAmount"/>",</xsl:for-each>"vergidahil":"<xsl:value-of select="n1:Invoice/cac:LegalMonetaryTotal/cbc:TaxInclusiveAmount"/>","odenecek":"<xsl:value-of select="n1:Invoice/cac:LegalMonetaryTotal/cbc:PayableAmount"/>"}</div>
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
                            <xsl:text>&#160;SAYIN</xsl:text>
                          </b>
                          <br/>
                          <xsl:if test="$Profil ='IHRACAT'">
                            <xsl:for-each select="n1:Invoice/cac:BuyerCustomerParty">
                              <xsl:call-template name="FirmaBilgisi"/>
                            </xsl:for-each>
                          </xsl:if>
                          <xsl:if test="$Profil !='IHRACAT'">
                            <xsl:for-each select="n1:Invoice/cac:AccountingCustomerParty">
                              <xsl:call-template name="FirmaBilgisi"/>
                            </xsl:for-each>
                          </xsl:if>
                          <br/>
                        </td>
                        <!--gib logosu altında logo istenirse-->
                        <td>
                          <br/>
                          <br/>
                          <!--#KASE#-->
                          <div id="dvdFrmSign"><img src="data:image/jpeg;base64,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" align="right"/></div>
                        </td>
                        <!--fatura bilgileri-->
                        <td align="right" valign="bottom" colspan="2" >
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
                                <td id="boldTd">Senaryo:</td>
                                <td>
                                  <xsl:for-each select="n1:Invoice/cbc:ProfileID">
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
                          <xsl:if test ="n1:Invoice/cbc:AccountingCost">
                            <br/>
                            <br/>
                            <!--sgk ile ilgili görsel-->
                            <table id="lineTable">
                              <tbody>
                                <tr>
                                  <td id="boldTd" align="left">Sağlık Fatura Tipi:</td>
                                  <td align="left">
                                    <xsl:for-each select="n1:Invoice/cbc:AccountingCost">
                                      <xsl:apply-templates/>
                                    </xsl:for-each>
                                  </td>
                                </tr>
                                <tr style="height:13px; ">
                                  <td id="boldTd">Mükellef Kodu:</td>
                                  <td align="left">
                                    <xsl:for-each select="n1:Invoice/cac:AdditionalDocumentReference[cbc:DocumentTypeCode='MUKELLEF_KODU']/cbc:DocumentType">
                                      <xsl:apply-templates/>
                                    </xsl:for-each>
                                  </td>
                                </tr>
                                <tr style="height:13px; ">
                                  <td id="boldTd">Mükellef Adı:</td>
                                  <td align="left">
                                    <xsl:for-each select="n1:Invoice/cac:AdditionalDocumentReference[cbc:DocumentTypeCode='MUKELLEF_ADI']/cbc:DocumentType">
                                      <xsl:apply-templates/>
                                    </xsl:for-each>
                                  </td>
                                </tr>
                                <tr style="height:13px; ">
                                  <td id="boldTd">Dosya No:</td>
                                  <td align="left">
                                    <xsl:for-each select="n1:Invoice/cac:AdditionalDocumentReference[cbc:DocumentTypeCode='DOSYA_NO']/cbc:DocumentType">
                                      <xsl:apply-templates/>
                                    </xsl:for-each>
                                  </td>
                                </tr>
                                <tr style="height:13px; ">
                                  <td id="boldTd">Dönem:</td>
                                  <td align="left">
                                    <xsl:for-each select="n1:Invoice">
                                      <xsl:for-each select="cac:InvoicePeriod/cbc:StartDate">
                                        <xsl:apply-templates/>
                                      </xsl:for-each>
                                      <span>
                                        <xsl:text> / </xsl:text>
                                      </span>
                                      <xsl:for-each select="cac:InvoicePeriod/cbc:EndDate">
                                        <xsl:apply-templates/>
                                      </xsl:for-each>
                                    </xsl:for-each>
                                  </td>
                                </tr>
                              </tbody>
                            </table>
                          </xsl:if>
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
                        <xsl:if test="//n1:Invoice/cbc:ProfileID='IHRACAT'">
                          <td style="width:30px;">
                            <xsl:text>Teslim Şartı</xsl:text>
                          </td>
                          <td style="width:30px;">
                            <xsl:text>Eşya Kap Cinsi</xsl:text>
                          </td>
                          <td style="width:30px;">
                            <xsl:text>Kap No</xsl:text>
                          </td>
                          <td style="width:30px;">
                            <xsl:text>Kap Adet</xsl:text>
                          </td>
                          <td style="width:30px;">
                            <xsl:text>Teslimat Yeri</xsl:text>
                          </td>
                          <td style="width:30px;">
                            <xsl:text>Gönderilme Şekli</xsl:text>
                          </td>
                          <td style="width:30px;">
                            <xsl:text>GTİP</xsl:text>
                          </td>
                        </xsl:if>
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
                  <!--dip toplam-->
                  <table style="width:800px;">
                    <tr>
                      <td style="width:500px;">&#160;</td>
                      <td style="width:300px;">
                        <table style="width:300px;margin-top:10px;margin-bottom:10px;" border ="1" id ="lineTable" align ="right">
                          <tbody>
                            <xsl:if	test="1=1">

                              <tr>
                                <td id="boldTd" style="width:200px;">&#160;Mal Hizmet Toplam Tutarı</td>
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
                            <!--<xsl:if test="not(contains(., 'KUR'))"></xsl:if>-->
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
                          <xsl:if test="//n1:Invoice/cac:BuyerCustomerParty/cac:Party/cac:PartyIdentification/cbc:ID[@schemeID='PARTYTYPE']='TAXFREE' and //n1:Invoice/cac:TaxRepresentativeParty/cac:PartyTaxScheme/cbc:ExemptionReasonCode">
                            <br/>
                            <b>&#160;&#160;&#160;&#160;&#160; VAT OFF - NO CASH REFUND </b>
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
      <xsl:if test="//n1:Invoice/cbc:ProfileID='IHRACAT'">
        <!--teslim şartı-->
        <td align="right">
          <xsl:text>&#160;</xsl:text>
          <xsl:for-each select="cac:Delivery/cac:DeliveryTerms/cbc:ID[@schemeID='INCOTERMS']">
            <xsl:text>&#160;</xsl:text>
            <xsl:apply-templates/>
          </xsl:for-each>
        </td>
        <!--eşya kap cinsi-->
        <td align="right">
          <xsl:text>&#160;</xsl:text>
          <xsl:for-each select="cac:Delivery/cac:Shipment/cac:TransportHandlingUnit/cac:ActualPackage/cbc:PackagingTypeCode">
            <xsl:text>&#160;</xsl:text>
            <xsl:variable name="PackagingTypeID" select="."/>
            <xsl:value-of select="document('')/*/lcl:PackagingTypeMap/e[@key=$PackagingTypeID]"/>
          </xsl:for-each>
        </td>
        <!--kap no-->
        <td align="right">
          <xsl:text>&#160;</xsl:text>
          <xsl:for-each select="cac:Delivery/cac:Shipment/cac:TransportHandlingUnit/cac:ActualPackage/cbc:ID">
            <xsl:text>&#160;</xsl:text>
            <xsl:apply-templates/>
          </xsl:for-each>
        </td>
        <!--kap adet-->
        <td align="right">
          <xsl:text>&#160;</xsl:text>
          <xsl:for-each select="cac:Delivery/cac:Shipment/cac:TransportHandlingUnit/cac:ActualPackage/cbc:Quantity">
            <xsl:text>&#160;</xsl:text>
            <xsl:apply-templates/>
          </xsl:for-each>
        </td>
        <!--teslim yeri-->
        <td align="right">
          <xsl:value-of select ="cac:Delivery/cac:DeliveryAddress/cbc:StreetName"/>
          <xsl:text>&#160;</xsl:text>
          <xsl:value-of select ="cac:Delivery/cac:DeliveryAddress/cbc:BuildingName"/>
          <xsl:text>&#160;</xsl:text>
          <xsl:value-of select ="cac:Delivery/cac:DeliveryAddress/cbc:BuildingNumber"/>
          <xsl:text>&#160;</xsl:text>
          <xsl:value-of select ="cac:Delivery/cac:DeliveryAddress/cbc:CitySubdivisionName"/>
          <xsl:text>&#160;</xsl:text>
          <xsl:value-of select ="cac:Delivery/cac:DeliveryAddress/cbc:CityName"/>
          <xsl:text>&#160;</xsl:text>
          <xsl:value-of select ="cac:Delivery/cac:DeliveryAddress/cbc:PostalZone"/>
          <xsl:text>&#160;</xsl:text>
          <br/>
          <xsl:value-of select ="cac:Delivery/cac:DeliveryAddress/cac:Country/cbc:Name"/>
          <xsl:text>&#160;</xsl:text>
        </td>
        <!--Gönderilme Şekli-->
        <td align="right">
          <xsl:text>&#160;</xsl:text>
          <xsl:for-each select="cac:Delivery/cac:Shipment/cac:ShipmentStage/cbc:TransportModeCode">
            <xsl:text>&#160;</xsl:text>
            <xsl:variable name="TransportModeID" select="."/>
            <xsl:value-of select="document('')/*/lcl:TransportModeMap/e[@key=$TransportModeID]"/>
          </xsl:for-each>
        </td>
        <!--gtip-->
        <td align="right">
          <xsl:text>&#160;</xsl:text>
          <xsl:for-each select="cac:Delivery/cac:Shipment/cac:GoodsItem/cbc:RequiredCustomsID">
            <xsl:text>&#160;</xsl:text>
            <xsl:apply-templates/>
          </xsl:for-each>
        </td>
      </xsl:if>
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
  <lcl:CountryMap>
    <e key="AF">Afganistan</e>
    <e key="DE">Almanya</e>
    <e key="AD">Andorra</e>
    <e key="AO">Angola</e>
    <e key="AG">Antigua ve Barbuda</e>
    <e key="AR">Arjantin</e>
    <e key="AL">Arnavutluk</e>
    <e key="AW">Aruba</e>
    <e key="AU">Avustralya</e>
    <e key="AT">Avusturya</e>
    <e key="AZ">Azerbaycan</e>
    <e key="BS">Bahamalar</e>
    <e key="BH">Bahreyn</e>
    <e key="BD">Bangladeş</e>
    <e key="BB">Barbados</e>
    <e key="EH">Batı Sahra (MA)</e>
    <e key="BE">Belçika</e>
    <e key="BZ">Belize</e>
    <e key="BJ">Benin</e>
    <e key="BM">Bermuda</e>
    <e key="BY">Beyaz Rusya</e>
    <e key="BT">Bhutan</e>
    <e key="AE">Birleşik Arap Emirlikleri</e>
    <e key="US">ABD</e>
    <e key="GB">Birleşik Krallık</e>
    <e key="BO">Bolivya</e>
    <e key="BA">Bosna-Hersek</e>
    <e key="BW">Botsvana</e>
    <e key="BR">Brezilya</e>
    <e key="BN">Bruney</e>
    <e key="BG">Bulgaristan</e>
    <e key="BF">Burkina Faso</e>
    <e key="BI">Burundi</e>
    <e key="TD">Çad</e>
    <e key="KY">Cayman Adaları</e>
    <e key="GI">Cebelitarık (GB)</e>
    <e key="CZ">Çek Cumhuriyeti</e>
    <e key="DZ">Cezayir</e>
    <e key="DJ">Cibuti</e>
    <e key="CN">Çin</e>
    <e key="DK">Danimarka</e>
    <e key="CD">Demokratik Kongo Cumhuriyeti</e>
    <e key="TL">Doğu Timor</e>
    <e key="DO">Dominik Cumhuriyeti</e>
    <e key="DM">Dominika</e>
    <e key="EC">Ekvador</e>
    <e key="GQ">Ekvator Ginesi</e>
    <e key="SV">El Salvador</e>
    <e key="ID">Endonezya</e>
    <e key="ER">Eritre</e>
    <e key="AM">Ermenistan</e>
    <e key="MF">Ermiş Martin (FR)</e>
    <e key="EE">Estonya</e>
    <e key="ET">Etiyopya</e>
    <e key="FK">Falkland Adaları</e>
    <e key="FO">Faroe Adaları (DK)</e>
    <e key="MA">Fas</e>
    <e key="FJ">Fiji</e>
    <e key="CI">Fildişi Sahili</e>
    <e key="PH">Filipinler</e>
    <e key="FI">Finlandiya</e>
    <e key="FR">Fransa</e>
    <e key="GF">Fransız Guyanası (FR)</e>
    <e key="PF">Fransız Polinezyası (FR)</e>
    <e key="GA">Gabon</e>
    <e key="GM">Gambiya</e>
    <e key="GH">Gana</e>
    <e key="GN">Gine</e>
    <e key="GW">Gine Bissau</e>
    <e key="GD">Grenada</e>
    <e key="GL">Grönland (DK)</e>
    <e key="GP">Guadeloupe (FR)</e>
    <e key="GT">Guatemala</e>
    <e key="GG">Guernsey (GB)</e>
    <e key="ZA">Güney Afrika</e>
    <e key="KR">Güney Kore</e>
    <e key="GE">Gürcistan</e>
    <e key="GY">Guyana</e>
    <e key="HT">Haiti</e>
    <e key="IN">Hindistan</e>
    <e key="HR">Hırvatistan</e>
    <e key="NL">Hollanda</e>
    <e key="HN">Honduras</e>
    <e key="HK">Hong Kong (CN)</e>
    <e key="VG">İngiliz Virjin Adaları</e>
    <e key="IQ">Irak</e>
    <e key="IR">İran</e>
    <e key="IE">İrlanda</e>
    <e key="ES">İspanya</e>
    <e key="IL">İsrail</e>
    <e key="SE">İsveç</e>
    <e key="CH">İsviçre</e>
    <e key="IT">İtalya</e>
    <e key="IS">İzlanda</e>
    <e key="JM">Jamaika</e>
    <e key="JP">Japonya</e>
    <e key="JE">Jersey (GB)</e>
    <e key="KH">Kamboçya</e>
    <e key="CM">Kamerun</e>
    <e key="CA">Kanada</e>
    <e key="ME">Karadağ</e>
    <e key="QA">Katar</e>
    <e key="KZ">Kazakistan</e>
    <e key="KE">Kenya</e>
    <e key="CY">Kıbrıs</e>
    <e key="KG">Kırgızistan</e>
    <e key="KI">Kiribati</e>
    <e key="CO">Kolombiya</e>
    <e key="KM">Komorlar</e>
    <e key="CG">Kongo Cumhuriyeti</e>
    <e key="KV">Kosova (RS)</e>
    <e key="CR">Kosta Rika</e>
    <e key="CU">Küba</e>
    <e key="KW">Kuveyt</e>
    <e key="KP">Kuzey Kore</e>
    <e key="LA">Laos</e>
    <e key="LS">Lesoto</e>
    <e key="LV">Letonya</e>
    <e key="LR">Liberya</e>
    <e key="LY">Libya</e>
    <e key="LI">Lihtenştayn</e>
    <e key="LT">Litvanya</e>
    <e key="LB">Lübnan</e>
    <e key="LU">Lüksemburg</e>
    <e key="HU">Macaristan</e>
    <e key="MG">Madagaskar</e>
    <e key="MO">Makao (CN)</e>
    <e key="MK">Makedonya</e>
    <e key="MW">Malavi</e>
    <e key="MV">Maldivler</e>
    <e key="MY">Malezya</e>
    <e key="ML">Mali</e>
    <e key="MT">Malta</e>
    <e key="IM">Man Adası (GB)</e>
    <e key="MH">Marshall Adaları</e>
    <e key="MQ">Martinique (FR)</e>
    <e key="MU">Mauritius</e>
    <e key="YT">Mayotte (FR)</e>
    <e key="MX">Meksika</e>
    <e key="FM">Mikronezya</e>
    <e key="EG">Mısır</e>
    <e key="MN">Moğolistan</e>
    <e key="MD">Moldova</e>
    <e key="MC">Monako</e>
    <e key="MR">Moritanya</e>
    <e key="MZ">Mozambik</e>
    <e key="MM">Myanmar</e>
    <e key="NA">Namibya</e>
    <e key="NR">Nauru</e>
    <e key="NP">Nepal</e>
    <e key="NE">Nijer</e>
    <e key="NG">Nijerya</e>
    <e key="NI">Nikaragua</e>
    <e key="NO">Norveç</e>
    <e key="CF">Orta Afrika Cumhuriyeti</e>
    <e key="UZ">Özbekistan</e>
    <e key="PK">Pakistan</e>
    <e key="PW">Palau</e>
    <e key="PA">Panama</e>
    <e key="PG">Papua Yeni Gine</e>
    <e key="PY">Paraguay</e>
    <e key="PE">Peru</e>
    <e key="PL">Polonya</e>
    <e key="PT">Portekiz</e>
    <e key="PR">Porto Riko (US)</e>
    <e key="RE">Réunion (FR)</e>
    <e key="RO">Romanya</e>
    <e key="RW">Ruanda</e>
    <e key="RU">Rusya</e>
    <e key="BL">Saint Barthélemy (FR)</e>
    <e key="KN">Saint Kitts ve Nevis</e>
    <e key="LC">Saint Lucia</e>
    <e key="PM">Saint Pierre ve Miquelon (FR)</e>
    <e key="VC">Saint Vincent ve Grenadinler</e>
    <e key="WS">Samoa</e>
    <e key="SM">San Marino</e>
    <e key="ST">São Tomé ve Príncipe</e>
    <e key="SN">Senegal</e>
    <e key="SC">Seyşeller</e>
    <e key="SL">Sierra Leone</e>
    <e key="CL">Şili</e>
    <e key="SG">Singapur</e>
    <e key="RS">Sırbistan</e>
    <e key="SK">Slovakya Cumhuriyeti</e>
    <e key="SI">Slovenya</e>
    <e key="SB">Solomon Adaları</e>
    <e key="SO">Somali</e>
    <e key="SS">South Sudan</e>
    <e key="SJ">Spitsbergen (NO)</e>
    <e key="LK">Sri Lanka</e>
    <e key="SD">Sudan</e>
    <e key="SR">Surinam</e>
    <e key="SY">Suriye</e>
    <e key="SA">Suudi Arabistan</e>
    <e key="SZ">Svaziland</e>
    <e key="TJ">Tacikistan</e>
    <e key="TZ">Tanzanya</e>
    <e key="TH">Tayland</e>
    <e key="TW">Tayvan</e>
    <e key="TG">Togo</e>
    <e key="TO">Tonga</e>
    <e key="TT">Trinidad ve Tobago</e>
    <e key="TN">Tunus</e>
    <e key="TR">Türkiye</e>
    <e key="TM">Türkmenistan</e>
    <e key="TC">Turks ve Caicos</e>
    <e key="TV">Tuvalu</e>
    <e key="UG">Uganda</e>
    <e key="UA">Ukrayna</e>
    <e key="OM">Umman</e>
    <e key="JO">Ürdün</e>
    <e key="UY">Uruguay</e>
    <e key="VU">Vanuatu</e>
    <e key="VA">Vatikan</e>
    <e key="VE">Venezuela</e>
    <e key="VN">Vietnam</e>
    <e key="WF">Wallis ve Futuna (FR)</e>
    <e key="YE">Yemen</e>
    <e key="NC">Yeni Kaledonya (FR)</e>
    <e key="NZ">Yeni Zelanda</e>
    <e key="CV">Yeşil Burun Adaları</e>
    <e key="GR">Yunanistan</e>
    <e key="ZM">Zambiya</e>
    <e key="ZW">Zimbabve</e>
  </lcl:CountryMap>
  <lcl:TransportModeMap>
    <e key="1">Denizyolu</e>
    <e key="2">Demiryolu</e>
    <e key="3">Karayolu</e>
    <e key="4">Havayolu</e>
    <e key="5">Posta</e>
    <e key="6">Çok araçlı</e>
    <e key="7">Sabit taşıma tesisleri</e>
    <e key="8">İç su taşımacılığı</e>
  </lcl:TransportModeMap>
  <lcl:PackagingTypeMap>
    <e key="1A">Bidon, çelik</e>
    <e key="1B">Bidon, alüminyum</e>
    <e key="1D">Bidon, kontrplak</e>
    <e key="1F">Konteynır, esnek</e>
    <e key="1G">Bidon, lifli</e>
    <e key="1W">Bidon, ahşap</e>
    <e key="2C">Varil, ahşap</e>
    <e key="3A">Beş galonluk bidon, çelik</e>
    <e key="3H">Beş galonluk bidon, plastik</e>
    <e key="43">Çanta, çok büyük</e>
    <e key="44">Çanta, polietilen</e>
    <e key="4A">Kutu, Çelik </e>
    <e key="4B">Kutu, Alüminyum</e>
    <e key="4C">Kutu, doğal tahta</e>
    <e key="4D">Kutu, kontrplak</e>
    <e key="4F">Kutu, işlenmiş tahta</e>
    <e key="4G">Kutu, lif</e>
    <e key="4H">Kutu, plastik</e>
    <e key="5H">Çanta, dokuma plastik</e>
    <e key="5L">Çanta, kumaş</e>
    <e key="5M">Çanta, kağıt</e>
    <e key="6H">Kompozit paketleme, plastik Kap</e>
    <e key="6P">Kompozit paketleme, cam Kap</e>
    <e key="7A">Kılıf, araba</e>
    <e key="7B">Kılıf, ahşap</e>
    <e key="8A">Palet, ahşap</e>
    <e key="8B">Kasa, ahşap</e>
    <e key="8C">Demet, ahşap</e>
    <e key="AA">Orta büyüklükte Konteynır, sert plastik</e>
    <e key="AB">Kap, lifli</e>
    <e key="AC">Kap, kağıt</e>
    <e key="AD">Kap, ahşap</e>
    <e key="AE">Aerosol</e>
    <e key="AF">Palet, modüler, Kolları : 80cms * 60cms</e>
    <e key="AG">Palet, Polimer plastik filmden üretilen</e>
    <e key="AH">Palet, 100cms * 110cms</e>
    <e key="AI">Çift Çeneli Kepçe</e>
    <e key="AJ">Koni</e>
    <e key="AL">Top</e>
    <e key="AM">Ampul, muhafazasız</e>
    <e key="AP">Ampul, muhafazalı</e>
    <e key="AT">Pülverizatör</e>
    <e key="AV">Kapsül</e>
    <e key="B4">Kayışla Bağlanmış</e>
    <e key="BA">Varil</e>
    <e key="BB">Makara</e>
    <e key="BC">Silindirik Kasa / Silindirik raf</e>
    <e key="BD">Tabla</e>
    <e key="BE">Demet</e>
    <e key="BF">Balon, muhafazasız</e>
    <e key="BG">Çanta</e>
    <e key="BH">Salkım</e>
    <e key="BI">Teneke Kova</e>
    <e key="BJ">Kova</e>
    <e key="BK">Basket</e>
    <e key="BL">Balya, Sıkıştırılmış</e>
    <e key="BM">Basin</e>
    <e key="BN">Balya, Sıkıştırılmamış</e>
    <e key="BO">Şişe, muhafazasız, silindirik</e>
    <e key="BP">Balon, muhafazalı</e>
    <e key="BQ">Şişe, muhafazalı silindirik</e>
    <e key="BR">Çubuk</e>
    <e key="BS">Şişe, muhafazasız, bombeli</e>
    <e key="BT">Bolt</e>
    <e key="BU">Butt</e>
    <e key="BV">Şişe, muhafazalı bombeli</e>
    <e key="BW">Kutu, Sıvı için</e>
    <e key="BX">Kutu</e>
    <e key="BY">Tabla, Demet/Salkım/Bağlam içinde</e>
    <e key="BZ">Bars, Demet/Salkım/Bağlam içinde</e>
    <e key="CA">Teneke Kutu, Dikdörtgen biçiminde</e>
    <e key="CB">Kasa, beer</e>
    <e key="CC">Yayık</e>
    <e key="CD">Teneke Kutu, with handle and spout</e>
    <e key="CE">Balıkçı Küfesi</e>
    <e key="CF">Kasar</e>
    <e key="CG">Serbet bırakılmamış</e>
    <e key="CH">Sandık</e>
    <e key="CI">Metal kap</e>
    <e key="CJ">Tabut</e>
    <e key="CK">Fıçı</e>
    <e key="CL">Bobin</e>
    <e key="CM">Kart</e>
    <e key="CN">Konteynır, not otherwise specified as transport equipment</e>
    <e key="CO">Cam balon, muhafazasız</e>
    <e key="CP">Cam balon, muhafazalı</e>
    <e key="CQ">Kartuş</e>
    <e key="CR">Kasa</e>
    <e key="CS">Kılıf</e>
    <e key="CT">Karton</e>
    <e key="CU">Bardak</e>
    <e key="CV">Kılıf</e>
    <e key="CW">Serbet bırakılmamış, Rulo</e>
    <e key="CX">Teneke Kutu, silindirik</e>
    <e key="CY">Cylinder</e>
    <e key="CZ">Canvas</e>
    <e key="DA">Kasa, çok katmanlı, plastik</e>
    <e key="DB">Kasa, çok katmanlı, ahşap</e>
    <e key="DC">Kasa, çok katmanlı, cardTabla</e>
    <e key="DG">Serbet bırakılmamış, Commonwealth Handling Equipment Pool (CHEP)</e>
    <e key="DH">Kutu, Commonwealth Handling Equipment Pool (CHEP), EuroKutu</e>
    <e key="DI">Bidon, Demir</e>
    <e key="DJ">Damacana, muhafazasız</e>
    <e key="DK">Kasa, Yük, cardTabla</e>
    <e key="DL">Kasa, Yük, plastik</e>
    <e key="DM">Kasa, Yük, ahşap</e>
    <e key="DN">Dispenser</e>
    <e key="DP">Damacana, muhafazalı</e>
    <e key="DR">Drum</e>
    <e key="DS">Tepsi, tek katmanlı Kılıfsız, plastik</e>
    <e key="DT">Tepsi, tek katmanlı Kılıfsız, ahşap</e>
    <e key="DU">Tepsi, tek katmanlı Kılıfsız, polystyrene</e>
    <e key="DV">Tepsi, tek katmanlı Kılıfsız, cardTabla</e>
    <e key="DW">Tepsi, çift katmanlı Kılıfsız, plastik</e>
    <e key="DX">Tepsi, çift katmanlı Kılıfsız, ahşap</e>
    <e key="DY">Tepsi, çift katmanlı Kılıfsız, cardTabla</e>
    <e key="EC">Çanta, plastik</e>
    <e key="ED">Kılıf, Palet tabanı ile</e>
    <e key="EE">Kılıf, Palet tabanı ile, ahşap</e>
    <e key="EF">Kılıf, Palet tabanı ile, cardTabla</e>
    <e key="EG">Kılıf, Palet tabanı ile, plastik</e>
    <e key="EH">Kılıf, Palet tabanı ile, metal</e>
    <e key="EI">Kılıf, isothermic</e>
    <e key="EN">Zarf</e>
    <e key="FB">Esnek Çanta</e>
    <e key="FC">Kasa, fruit</e>
    <e key="FD">Kasa, Çerçeved</e>
    <e key="FE">Esnek tank</e>
    <e key="FI">Ufak yağ fıçısı</e>
    <e key="FL">Termos</e>
    <e key="FO">Manevra sandığı</e>
    <e key="FP">Filmpaket</e>
    <e key="FR">Çerçeve</e>
    <e key="FT">Foodtainer</e>
    <e key="FW">El arabası, düz platform</e>
    <e key="FX">Çanta, esnek Konteynır</e>
    <e key="GB">Şişe, gas</e>
    <e key="GI">Kiriş</e>
    <e key="GL">Konteynır, galon</e>
    <e key="GR">Kap, cam</e>
    <e key="GU">Tepsi, istiflenmiş düz ögeleri containing horizontally stacked flat items</e>
    <e key="GY">Çanta, gunny</e>
    <e key="GZ">Girders, Demet/Salkım/Bağlam içinde</e>
    <e key="HA">Basket, with handle, plastik</e>
    <e key="HB">Basket, with handle, ahşap</e>
    <e key="HC">Basket, with handle, cardTabla</e>
    <e key="HG">Hogshead</e>
    <e key="HN">Hanger</e>
    <e key="HR">Sepet</e>
    <e key="IA">Package, display, ahşap</e>
    <e key="IB">Karton Koli</e>
    <e key="IC">Package, display, plastik</e>
    <e key="ID">Package, display, metal</e>
    <e key="IE">Package, show</e>
    <e key="IF">Package, flow</e>
    <e key="IG">Package, kağıt wrapped</e>
    <e key="IH">Bidon, plastik</e>
    <e key="IK">Package, cardTabla, with Şişe grip-holes</e>
    <e key="IL">Tepsi, sert, lidded stackable (CEN TS 14482:2002)</e>
    <e key="IN">Küfe</e>
    <e key="IZ">Küfeler, Demet/Salkım/Bağlam içinde</e>
    <e key="JB">Çanta, jumbo</e>
    <e key="JC">Beş galonluk bidon, Dikdörtgen biçiminde</e>
    <e key="JG">Sürahi</e>
    <e key="JR">Kavanoz</e>
    <e key="JT">Çanta, Jüt</e>
    <e key="JY">Beş galonluk bidon, silindirik</e>
    <e key="KG">Küçük Fıçı</e>
    <e key="KI">Takım</e>
    <e key="LE">Bagaj</e>
    <e key="LG">Kütük</e>
    <e key="LT">Yığın</e>
    <e key="LU">Kulp</e>
    <e key="LV">Yük Vagonu</e>
    <e key="LZ">Kütük, Demet/Salkım/Bağlam içinde</e>
    <e key="MA">Kasa, Metal</e>
    <e key="MB">Çanta, multiply</e>
    <e key="MC">Kasa, milk</e>
    <e key="ME">Konteynır, metal</e>
    <e key="MR">Kap, metal</e>
    <e key="MS">Çuval, Çok Katlı</e>
    <e key="MT">Keçe</e>
    <e key="MW">Kap, plastik wrapped</e>
    <e key="MX">Kibrit Kutusu</e>
    <e key="NA">Hazır değil</e>
    <e key="NE">Paketlenmemiş</e>
    <e key="NF">Paketlenmemiş tek ünite</e>
    <e key="NG">Paketlenmemiş Çok ünite</e>
    <e key="NS">Yuva</e>
    <e key="NT">Ağ</e>
    <e key="NU">Ağ, Tüp, plastik</e>
    <e key="NV">Ağ, Tüp, kumaş</e>
    <e key="OA">Palet, CHEP 40 cm x 60 cm</e>
    <e key="OB">Palet, CHEP 80 cm x 120 cm</e>
    <e key="OC">Palet, CHEP 100 cm x 120 cm</e>
    <e key="OD">Palet, AS 4068-1993</e>
    <e key="OE">Palet, ISO T11</e>
    <e key="OF">Platform, Belirtilmemiş boyut ve ağırlıkta</e>
    <e key="OK">Blok</e>
    <e key="OT">Kutu, Sekizgen</e>
    <e key="OU">Konteynır, outer</e>
    <e key="P2">Yassı Kap</e>
    <e key="PA">Paket</e>
    <e key="PB">Palet, Ucu açık Kutu ve Paletile birleşmiş kutu</e>
    <e key="PC">Parsel</e>
    <e key="PD">Palet, modüler, Kolları : 80cms * 100cms</e>
    <e key="PE">Palet, modüler, Kolları : 80cms * 120cms</e>
    <e key="PF">Kafes</e>
    <e key="PG">Plaka</e>
    <e key="PH">Sürahi</e>
    <e key="PI">Boru</e>
    <e key="PJ">Sepet</e>
    <e key="PK">Paket</e>
    <e key="PL">Gerdel</e>
    <e key="PN">Kalas</e>
    <e key="PO">Torba</e>
    <e key="PP">Parça</e>
    <e key="PR">Kap, plastik</e>
    <e key="PT">Testi</e>
    <e key="PU">Tepsi</e>
    <e key="PV">Boru, Demet/Salkım/Bağlam içinde</e>
    <e key="PX">Palet</e>
    <e key="PY">Plakalar, Demet/Salkım/Bağlam içinde</e>
    <e key="PZ">Kalaslar, Demet/Salkım/Bağlam içinde</e>
    <e key="QA">Bidon, çelik, non-removable head</e>
    <e key="QB">Bidon, çelik, removable head</e>
    <e key="QC">Bidon, alüminyum, non-removable head</e>
    <e key="QD">Bidon, alüminyum, removable head</e>
    <e key="QF">Bidon, plastik, non-removable head</e>
    <e key="QG">Bidon, plastik, removable head</e>
    <e key="QH">Varil, ahşap, bung type</e>
    <e key="QJ">Varil, ahşap, removable head</e>
    <e key="QK">Beş galonluk bidon, çelik, non-removable head</e>
    <e key="QL">Beş galonluk bidon, çelik, removable head</e>
    <e key="QM">Beş galonluk bidon, plastik, non-removable head</e>
    <e key="QN">Beş galonluk bidon, plastik, removable head</e>
    <e key="QP">Kutu, ahşap, doğal tahta, ordinary</e>
    <e key="QQ">Kutu, ahşap, doğal tahta, with sift proof walls</e>
    <e key="QR">Kutu, plastik, expanded</e>
    <e key="QS">Kutu, plastik, katı</e>
    <e key="RD">Çubuk</e>
    <e key="RG">Halka</e>
    <e key="RJ">Raf, Giysi Askısı</e>
    <e key="RK">Raf</e>
    <e key="RL">Makara</e>
    <e key="RO">Rulo</e>
    <e key="RT">Rednet</e>
    <e key="RZ">Çubuklar, Demet/Salkım/Bağlam içinde</e>
    <e key="SA">Çuval</e>
    <e key="SB">Kalın Levha</e>
    <e key="SC">Kasa, shallow</e>
    <e key="SD">Torna Mil</e>
    <e key="SE">Denizci Sandığı</e>
    <e key="SH">Küçük Kese</e>
    <e key="SI">Izgara</e>
    <e key="SK">Kılıf, iskelet</e>
    <e key="SL">Plastik, yüksek laminasyonlu karton veya oluklu fiberden yapılan taşıma paleti</e>
    <e key="SM">Sac Levha</e>
    <e key="SO">Sargı</e>
    <e key="SP">Tabaka, plastik wrapping</e>
    <e key="SS">Kılıf, çelik</e>
    <e key="ST">Tabaka</e>
    <e key="SU">Suit Kılıf</e>
    <e key="SV">Envelope, çelik</e>
    <e key="SW">Polimer plastik filmden üretilen</e>
    <e key="SX">Takım</e>
    <e key="SY">Sleeve</e>
    <e key="SZ">Tabakalar, Demet/Salkım/Bağlam içinde</e>
    <e key="T1">Tablet</e>
    <e key="TB">Küvet</e>
    <e key="TC">Çay-Sandık</e>
    <e key="TD">Tüp, collapsible</e>
    <e key="TE">Tekerlek</e>
    <e key="TG">Tank Konteynır, generic</e>
    <e key="TI">Tierce</e>
    <e key="TK">Tank, Dikdörtgen biçiminde</e>
    <e key="TL">Küvet, Kapaklı</e>
    <e key="TN">Kalay Kutu</e>
    <e key="TO">Fıçı</e>
    <e key="TR">Kovan</e>
    <e key="TS">Bağlam</e>
    <e key="TT">Çanta, tote</e>
    <e key="TU">Tüp</e>
    <e key="TV">Tüp, with nozzle</e>
    <e key="TW">Palet, triwall</e>
    <e key="TY">Tank, silindirik</e>
    <e key="TZ">Tüpler, Demet/Salkım/Bağlam içinde</e>
    <e key="UC">Serbest bırakılmış</e>
    <e key="UN">Ünite</e>
    <e key="VA">Tekne</e>
    <e key="VG">Yük, gas (at 1031 mbar and 15Â°C)</e>
    <e key="VI">Küçük Şişe</e>
    <e key="VK">Paket Vaogn</e>
    <e key="VL">Yük, sıvı</e>
    <e key="VO">Yük, katı, büyük parçalı (Â“nodulesÂ”)</e>
    <e key="VP">Vakumlanmış Paket</e>
    <e key="VQ">Yük, sıvılaştırılmış gaz(anormal Isı/Basınçta)</e>
    <e key="VN">Uzun Araç</e>
    <e key="VR">Yük, katı, tane parçalı (Â“grainsÂ”)</e>
    <e key="VS">Yük, hurda metal</e>
    <e key="VY">Yük, katı, fine parçalı (Â“powdersÂ”)</e>
    <e key="WA">Orta büyüklükte Konteynır</e>
    <e key="WB">Hasır Şişe</e>
    <e key="WC">Orta büyüklükte Konteynır, çelik</e>
    <e key="WD">Orta büyüklükte Konteynır, alüminyum</e>
    <e key="WF">Orta büyüklükte Konteynır, metal</e>
    <e key="WG">Orta büyüklükte Konteynır, çelik, basınçlı > 10 kpa</e>
    <e key="WH">Orta büyüklükte Konteynır, alüminyum, basınçlı > 10 kpa</e>
    <e key="WJ">Orta büyüklükte Konteynır, metal, Basınç 10 kpa</e>
    <e key="WK">Orta büyüklükte Konteynır, çelik, sıvı</e>
    <e key="WL">Orta büyüklükte Konteynır, alüminyum, sıvı</e>
    <e key="WM">Orta büyüklükte Konteynır, metal, sıvı</e>
    <e key="WN">Orta büyüklükte Konteynır, dokuma plastik, without coat/liner</e>
    <e key="WP">Orta büyüklükte Konteynır, dokuma plastik, coated</e>
    <e key="WQ">Orta büyüklükte Konteynır, dokuma plastik, with liner</e>
    <e key="WR">Orta büyüklükte Konteynır, dokuma plastik, coated and liner</e>
    <e key="WS">Orta büyüklükte Konteynır, plastik film</e>
    <e key="WT">Orta büyüklükte Konteynır, kumaş with out coat/liner</e>
    <e key="WU">Orta büyüklükte Konteynır, doğal tahta, with inner liner</e>
    <e key="WV">Orta büyüklükte Konteynır, kumaş, coated</e>
    <e key="WW">Orta büyüklükte Konteynır, kumaş, with liner</e>
    <e key="WX">Orta büyüklükte Konteynır, kumaş, coated and liner</e>
    <e key="WY">Orta büyüklükte Konteynır, kontrplak, with inner liner</e>
    <e key="WZ">Orta büyüklükte Konteynır, yapılandırılmış tahta, with inner liner</e>
    <e key="XA">Çanta, dokuma plastik, without inner coat/liner</e>
    <e key="XB">Çanta, dokuma plastik, sift proof</e>
    <e key="XC">Çanta, dokuma plastik, Su geçirmez</e>
    <e key="XD">Çanta, plastiks film</e>
    <e key="XF">Çanta, kumaş, without inner coat/liner</e>
    <e key="XG">Çanta, kumaş, sift proof</e>
    <e key="XH">Çanta, kumaş, Su geçirmez</e>
    <e key="XJ">Çanta, kağıt, Çok Katlı</e>
    <e key="XK">Çanta, kağıt, Çok Katlı, Su geçirmez</e>
    <e key="YA">Kompozit paketleme, çelik bidon içinde plastik Kap</e>
    <e key="YB">Kompozit paketleme, çelik Kasa Kutu içinde plastik Kap</e>
    <e key="YC">Kompozit paketleme, alüminyum bidon içinde plastik Kap</e>
    <e key="YD">Kompozit paketleme, alüminyum Kasa içinde plastik Kap</e>
    <e key="YF">Kompozit paketleme, ahşap Kutu içinde plastik Kap</e>
    <e key="YG">Kompozit paketleme, kontrplak bidon içinde plastik Kap</e>
    <e key="YH">Kompozit paketleme, kontrplak Kutu içinde plastik Kap</e>
    <e key="YJ">Kompozit paketleme, lifli bidon içinde plastik Kap</e>
    <e key="YK">Kompozit paketleme, lif Kutu içinde plastik Kap</e>
    <e key="YL">Kompozit paketleme, plastik bidon içinde plastik Kap</e>
    <e key="YM">Kompozit paketleme, katı plastik Kutu içinde plastik Kap</e>
    <e key="YN">Kompozit paketleme, çelik bidon içinde cam Kap</e>
    <e key="YP">Kompozit paketleme, çelik Kasa Kutu içinde cam Kap</e>
    <e key="YQ">Kompozit paketleme, alüminyum bidon içinde cam Kap</e>
    <e key="YR">Kompozit paketleme, alüminyum Kasa içinde cam Kap</e>
    <e key="YS">Kompozit paketleme, ahşap Kutu içinde cam Kap</e>
    <e key="YT">Kompozit paketleme, kontrplak bidon içinde cam Kap</e>
    <e key="YV">Kompozit paketleme, Hasırişi Sepet içinde cam Kap</e>
    <e key="YW">Kompozit paketleme, lifli bidon içinde cam Kap</e>
    <e key="YX">Kompozit paketleme, lif Kutu içinde cam Kap</e>
    <e key="YY">Kompozit paketleme, expandable plastik pack içinde cam Kap</e>
    <e key="YZ">Kompozit paketleme, katı plastik pack içinde cam Kap</e>
    <e key="ZA">Orta büyüklükte Konteynır, kağıt, Çok Katlı</e>
    <e key="ZB">Çanta, büyük</e>
    <e key="ZC">Orta büyüklükte Konteynır, kağıt, Çok Katlı, Su geçirmez</e>
    <e key="ZD">Orta büyüklükte Konteynır, sert plastik, with structural equipment, katıs</e>
    <e key="ZF">Orta büyüklükte Konteynır, sert plastik, freestanding, katıs</e>
    <e key="ZG">Orta büyüklükte Konteynır, sert plastik, with structural equipment, basınçlı</e>
    <e key="ZH">Orta büyüklükte Konteynır, sert plastik, freestanding, basınçlı</e>
    <e key="ZJ">Orta büyüklükte Konteynır, sert plastik, with structural equipment, Sıvı</e>
    <e key="ZK">Orta büyüklükte Konteynır, sert plastik, freestanding, Sıvı</e>
    <e key="ZL">Orta büyüklükte Konteynır, Kompozit, sert plastik, katı</e>
    <e key="ZM">Orta büyüklükte Konteynır, Kompozit, esnek plastik, katı</e>
    <e key="ZN">Orta büyüklükte Konteynır, Kompozit, sert plastik, basınçlı</e>
    <e key="ZP">Orta büyüklükte Konteynır, Kompozit, esnek plastik, basınçlı</e>
    <e key="ZQ">Orta büyüklükte Konteynır, Kompozit, sert plastik, Sıvı</e>
    <e key="ZR">Orta büyüklükte Konteynır, Kompozit, esnek plastik, Sıvı</e>
    <e key="ZS">Orta büyüklükte Konteynır, Kompozit</e>
    <e key="ZT">Orta büyüklükte Konteynır, lif</e>
    <e key="ZU">Orta büyüklükte Konteynır, esnek</e>
    <e key="ZV">Orta büyüklükte Konteynır, metal, other than çelik</e>
    <e key="ZW">Orta büyüklükte Konteynır, doğal tahta</e>
    <e key="ZX">Orta büyüklükte Konteynır, kontrplak</e>
    <e key="ZY">Orta büyüklükte Konteynır, yapılandırılmış tahta</e>
  </lcl:PackagingTypeMap>

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
                    <xsl:value-of select="document('')/*/lcl:CountryMap/e[@key=$NationalityID]"/>
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


